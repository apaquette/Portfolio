{
  description = ".NET Blazor development environment (NixOS flake)";

  inputs = {
      nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
      flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = {self, nixpkgs, flake-utils }:
      flake-utils.lib.eachDefaultSystem (system:
          let
  pkgs = import nixpkgs {
    inherit system;
    config.allowUnfree = true;
  };

  dotnet =
    with pkgs.dotnetCorePackages;
    combinePackages [
      sdk_9_0
      sdk_8_0
    ];
        in {
        devShells.default = pkgs.mkShell {
            packages = [
            dotnet
            pkgs.git
            ];

            shellHook = ''
                export DOTNET_ROOT="${dotnet}/share/dotnet"
                export DOTNET_ROOT_X64="$DOTNET_ROOT"

                export DOTNET_CLI_HOME="$HOME/.dotnet"

                # The dotnet executable is exposed in this directory.
                export PATH="${dotnet}/bin:$PATH"

                export DOTNET_TOOLS="$DOTNET_CLI_HOME/tools"
                export PATH="$DOTNET_TOOLS:$PATH"

                mkdir -p "$DOTNET_TOOLS"

                install_dotnet_tool() {
                    local package="$1"
                    local command="$2"

                    if ! command -v "$command" >/dev/null 2>&1; then
                    echo "Installing $package..."
                    dotnet tool install --global "$package"
                    fi
                }

                install_dotnet_tool \
                    "dotnet-reportgenerator-globaltool" \
                    "reportgenerator"

                echo ".NET dev shell ready"
                echo "dotnet: $(dotnet --version)"
                echo "reportgenerator: $(reportgenerator --version)"

                exec fish
                '';
            };
        }

    );
}