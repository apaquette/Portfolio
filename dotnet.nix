with import <nixpkgs> {config.allowUnfree = true;};
  mkShell rec {
    name = "main-dev-shell";

    dotnet = with dotnetCorePackages;
      combinePackages [
        sdk_8_0
        sdk_9_0
      ];

    deps = [
      zlib
      openssl
      stdenv.cc.cc.lib
      dotnet
    ];
    
    buildInputs = [
      pkgs.fish
      dotnet
      git
      
      (pkgs.vscode.fhsWithPackages (ps: with ps; [
        dotnet
        nix
      ]) // { # this bit doesn't seem to work :(
        vscodeExtensions = with pkgs.vscode-extensions.override; [
          ms-dotnettools.csdevkit
          ms-dotnettools.csharp
          ms-dotnettools.vscode-dotnet-runtime
          github.vscode-github-actions
          github.copilot-chat

          pkief.material-icon-theme
          bbenoist.nix
        ];
      })
    ];

    packages = [
      fontconfig # Avalonia
      lttng-ust_2_12 # Avalonia
    ];

    shellHook = ''
      export DOTNET_ROOT="${dotnet}/share/dotnet"
      export DOTNET_HOST_ROOT="${dotnet}/share/dotnet"
      export PATH="$DOTNET_ROOT/bin:$PATH"
      export PATH="$PATH:/home/apaquette/.dotnet/tools"

      export LD_LIBRARY_PATH=${pkgs.lib.makeLibraryPath [
        pkgs.fontconfig # Avalonia
        pkgs.xorg.libX11 # Avalonia
        pkgs.xorg.libICE # Avalonia
        pkgs.xorg.libSM # Avalonia
      ]}

      # Install Coverlet and ReportGenerator
      dotnet tool install --global coverlet.console
      dotnet tool install --global dotnet-reportgenerator-globaltool

      exec fish
    '';
  }
