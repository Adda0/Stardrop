﻿using Semver;
using Stardrop.Models;
using Stardrop.Models.SMAPI;
using Stardrop.Models.SMAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stardrop.Utilities.External
{
    static class SMAPI
    {
        internal static bool IsRunning = false;
        internal static Process Process;

        public static ProcessStartInfo GetPrepareProcess(bool hideConsole)
        {
            var arguments = String.Empty;
            var smapiInfo = new FileInfo(Pathing.GetSmapiPath());
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) is true)
            {
                // arguments = $"-c \"SMAPI_MODS_PATH='{Pathing.GetSelectedModsFolderPath()}' '{Pathing.GetSmapiPath().Replace("StardewModdingAPI.dll", "StardewValley")}'\"";
                arguments = $"-c \"'{Pathing.GetSmapiPath().Replace("StardewModdingAPI.dll", "StardewModdingAPI")}' --mods-path '{Pathing.GetSelectedModsFolderPath()}'\"";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) is true)
            {
                arguments = $"-c \"'{Pathing.GetSmapiPath().Replace("StardewModdingAPI.dll", "StardewModdingAPI")}' --mods-path '{Pathing.GetSelectedModsFolderPath()}'\"";
            }

            Program.helper.Log($"Starting SMAPI with the following arguments: {arguments}");
            var processInfo = new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? smapiInfo.FullName : "/bin/bash",
                Arguments = arguments,
                WorkingDirectory = smapiInfo.DirectoryName,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = hideConsole,
                UseShellExecute = false
            };
            processInfo.EnvironmentVariables["SMAPI_MODS_PATH"] = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) is false ? $"'{Pathing.GetSelectedModsFolderPath()}'" : Pathing.GetSelectedModsFolderPath();

            return processInfo;
        }

        public static string GetProcessName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "StardewModdingA";
            }

            return "StardewModdingAPI";
        }

        public async static Task<List<ModEntry>> GetModUpdateData(GameDetails gameDetails, List<Mod> mods)
        {
            List<ModSearchEntry> searchEntries = new List<ModSearchEntry>();
            foreach (var mod in mods.Where(m => m.HasValidVersion() && m.HasUpdateKeys()))
            {
                searchEntries.Add(new ModSearchEntry(mod.UniqueId, mod.Version, mod.Manifest.UpdateKeys));
            }
            foreach (var requirementKey in mods.SelectMany(m => m.Requirements))
            {
                if (!searchEntries.Any(e => e.Id.Equals(requirementKey.UniqueID, StringComparison.OrdinalIgnoreCase)))
                {
                    searchEntries.Add(new ModSearchEntry() { Id = requirementKey.UniqueID });
                }
            }

            // Create the body to be sent via the POST request
            ModSearchData searchData = new ModSearchData(searchEntries, gameDetails.SmapiVersion, gameDetails.GameVersion, gameDetails.System.ToString(), true);

            // Create a throwaway client
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Application-Name", "Stardrop");
            client.DefaultRequestHeaders.Add("Application-Version", Program.ApplicationVersion);
            client.DefaultRequestHeaders.Add("User-Agent", $"Stardrop/{Program.ApplicationVersion} {Environment.OSVersion}");

            var parsedRequest = JsonSerializer.Serialize(searchData, new JsonSerializerOptions() { WriteIndented = true, IgnoreNullValues = true });
            var requestPackage = new StringContent(parsedRequest, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://smapi.io/api/v3.0/mods", requestPackage);

            List<ModEntry> modUpdateData = new List<ModEntry>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Content is not null)
            {
                // In the name of the Nine Divines, why is JsonSerializer.Deserialize case sensitive by default???
                string content = await response.Content.ReadAsStringAsync();
                modUpdateData = JsonSerializer.Deserialize<List<ModEntry>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (modUpdateData is null || modUpdateData.Count == 0)
                {
                    Program.helper.Log($"Mod update data was not parsable from smapi.io");
                    Program.helper.Log($"Response from smapi.io:\n{content}");
                    Program.helper.Log($"Our request to smapi.io:\n{parsedRequest}");
                }
            }
            else
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Program.helper.Log($"Bad status given from smapi.io: {response.StatusCode}");
                    if (response.Content is not null)
                    {
                        Program.helper.Log($"Response from smapi.io:\n{await response.Content.ReadAsStringAsync()}");
                    }
                }
                else if (response.Content is null)
                {
                    Program.helper.Log($"No response from smapi.io!");
                }
                else
                {
                    Program.helper.Log($"Error getting mod update data from smapi.io!");
                }

                Program.helper.Log($"Our request to smapi.io:\n{parsedRequest}");
            }

            client.Dispose();

            return modUpdateData;
        }

        internal static SemVersion? GetVersion()
        {
            AssemblyName smapiAssembly = AssemblyName.GetAssemblyName(Path.Combine(Pathing.defaultGamePath, "StardewModdingAPI.dll"));

            if (smapiAssembly is null || smapiAssembly.Version is null)
            {
                return null;
            }

            return SemVersion.Parse($"{smapiAssembly.Version.Major}.{smapiAssembly.Version.Minor}.{smapiAssembly.Version.Build}", SemVersionStyles.Any);
        }
    }
}
