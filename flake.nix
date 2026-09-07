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

              dotnet = pkgs.dotnet-sdk_9;

          in {
              devShells.default = pkgs.mkShell {
                  packages = [
                      dotnet
                      pkgs.git
                  ];

                  shellHook = ''
                  export DOTNET_ROOT=${dotnet}
                  export DOTNET_CLI_HOME=$HOME/.dotnet
                  export PATH=$DOTNET_ROOT/bin:$PATH

                  echo ".NET Blazor dev shell ready"
                  echo "dotnet: $(dotnet --version)"

                  # Install Coverlet and ReportGenerator
                  dotnet tool update --global coverlet.console
                  dotnet tool update --global dotnet-reportgenerator-globaltool
                  
                  # Git Configuration
                  git config --global user.name "Alex Paquette"
                  git config --global user.email "alex.paquette@example.com"

                  exec fish
                ''
            };
        }
    );
}