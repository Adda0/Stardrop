{
  lib,
  stdenv,
  fetchFromGitHub,
  python3,
  flatpak,
  makeWrapper,
}:

stdenv.mkDerivation rec {
  pname = "flatpak-dotnet-generator";
  version = "unstable-2026-03-25";

  src = fetchFromGitHub {
    owner = "flatpak";
    repo = "flatpak-builder-tools";
    rev = "caca92bc6898ea40d6521b581fef2402ba3b3aae"; # Latest commit on master
    hash = "sha256-4/kg3HnUBFqUy9wQ/SabK4RQ9FwxZbFyOpWQ++8AIPI="; # Update this with the actual hash using `nix build` or `nix hash git`
  };

  # Enter the dotnet directory automatically
  sourceRoot = "${src.name}/dotnet";

  nativeBuildInputs = [ makeWrapper ];
  buildInputs = [ python3 ];

  installPhase = ''
    runHook preInstall

    install -Dm755 flatpak-dotnet-generator.py $out/bin/flatpak-dotnet-generator

    # Ensure the script can find flatpak at runtime
    wrapProgram $out/bin/flatpak-dotnet-generator \
      --prefix PATH : ${lib.makeBinPath [ flatpak ]}

    runHook postInstall
  '';

  meta = with lib; {
    description = "Script to generate flatpak-builder manifest from csproj files";
    homepage = "https://github.com/flatpak/flatpak-builder-tools/tree/master/dotnet";
    license = licenses.mit;
    mainProgram = "flatpak-dotnet-generator";
    maintainers = [ ];
  };
}
