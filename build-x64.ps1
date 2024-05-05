#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Build Text-Grab
.DESCRIPTION
    ...
.NOTES
    ...
.LINK
    ...
.EXAMPLE
    ./build-x64.ps1 -Verbose
#>


$BuildPath = "$PSScriptRoot/bld/x64"
$BuildPathSC = "$PSScriptRoot/bld/x64/Text-Grab-Self-Contained"
$Version = Get-Date -Format 'yyyy-MM-dd' # 2020-11-1
$VersionDot = $Version -replace '-', '.'
$Project = 'Text-Grab'
$Archive = "$BuildPath/$Project-$Version.zip"
$ArchiveSC = "$BuildPath/$Project-Self-Contained-$Version.zip"

# Clean up
if (Test-Path -Path $BuildPath) {
    Remove-Item $BuildPath -Recurse
}

New-Item $BuildPath -ItemType Directory -ea 0

# Dotnet restore and build
dotnet publish "$PSScriptRoot/$Project/$Project.csproj" `
    --runtime win-x64 `
    --self-contained false `
    -c Release `
    -v minimal `
    -o $BuildPath `
    -p:PublishReadyToRun=true `
    -p:PublishSingleFile=true `
    -p:CopyOutputSymbolsToPublishDirectory=false `
    -p:Version=$VersionDot `
    --nologo

# Archive Build
Compress-Archive -Path "$BuildPath/$Project.exe" -DestinationPath $Archive

# Dotnet restore and build
dotnet publish "$PSScriptRoot/$Project/$Project.csproj" `
    --runtime win-x64 `
    --self-contained true `
    -c Release `
    -v minimal `
    -o $BuildPathSC `
    -p:PublishReadyToRun=true `
    -p:PublishSingleFile=true `
    -p:CopyOutputSymbolsToPublishDirectory=false `
    -p:Version=$VersionDot `
    --nologo

# Archive Build
Compress-Archive -Path "$BuildPathSC" -DestinationPath $ArchiveSC
