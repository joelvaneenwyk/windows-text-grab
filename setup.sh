#!/usr/bin/sh

set -eax

###################################
# Prerequisites

_install() {
    out="$(mktemp -d)"
    cd "$out" || (
        echo "Failed to change directory: '$out'"
        exit 55
    )
    
    # Get the version of Ubuntu
    # shellcheck disable=SC1091
    if command -v pwsh >/dev/null; then
        echo "PowerShell (\`pwsh\`) is already installed."
    else
        if ! command -v wget >/dev/null; then
            # Update the list of packages
            sudo apt-get update

            # Install pre-requisite packages.
            sudo apt-get install -y --no-install-recommends \
                wget apt-transport-https software-properties-common
        fi

        if . /etc/os-release; then
            # Download the Microsoft repository keys
            wget -q "https://packages.microsoft.com/config/ubuntu/${VERSION_ID:-}/packages-microsoft-prod.deb"

            # Register the Microsoft repository keys
            sudo dpkg -i packages-microsoft-prod.deb

            # Delete the Microsoft repository keys file
            rm packages-microsoft-prod.deb

            # Update the list of packages after we added packages.microsoft.com
            sudo apt-get update

            ###################################
            # Install PowerShell
            sudo apt-get install -y powershell
        fi
    fi

    if command -v dotnet >/dev/null; then
        echo "\`dotnet\` is already installed."
    else
        wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
        chmod +x dotnet-install.sh
        ./dotnet-install.sh --version latest
        rm dotnet-install.sh
    fi
}

_install
