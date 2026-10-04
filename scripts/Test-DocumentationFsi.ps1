<#
.SYNOPSIS
Compiles the documented F# Interactive example against freshly packed packages.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$PackageDirectory,

    [Parameter(Mandatory)]
    [string]$PackageVersion
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$documentation = Get-Content (Join-Path $repositoryRoot 'docs/docs/examples/fsharp-interactive.md') -Raw
$fence = @([regex]::Matches($documentation, '(?s)```fsharp\s*\n(.*?)```') |
    Where-Object { $_.Groups[1].Value.Contains('type UpdateDotnetWorkloads') })
if ($fence.Count -ne 1) {
    throw 'Expected exactly one F# Interactive example.'
}

$lines = $fence[0].Groups[1].Value.Replace("`r`n", "`n").Split("`n")
$indent = ($lines | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
    ForEach-Object { $_.Length - $_.TrimStart(' ').Length } | Measure-Object -Minimum).Minimum
$code = ($lines | ForEach-Object {
    if ([string]::IsNullOrWhiteSpace($_)) { '' } else { $_.Substring($indent) }
}) -join "`n"
$reference = '#r "nuget: ModularPipelines.DotNet, 4.*"'
if (-not $code.Contains($reference)) {
    throw 'The documented DotNet package reference has changed; update the FSI validation.'
}
$code = $code.Replace($reference, ('#r "nuget: ModularPipelines.DotNet, ' + $PackageVersion + '"'))
$executionStart = $code.IndexOf('let args = System.Environment.GetCommandLineArgs()', [StringComparison]::Ordinal)
if ($executionStart -lt 0) {
    throw 'Cannot locate the example execution section.'
}

# FSI type-checks this function without running workload updates or SDK checks.
$declarations = $code.Substring(0, $executionStart)
$execution = ($code.Substring($executionStart).Split("`n") | ForEach-Object { '    ' + $_ }) -join "`n"
$temporaryRoot = Join-Path ([IO.Path]::GetTempPath()) ('modularpipelines-fsi-' + [guid]::NewGuid().ToString('N'))
$previousPackages = $env:NUGET_PACKAGES
try {
    $feed = New-Item -ItemType Directory -Path (Join-Path $temporaryRoot 'feed')
    foreach ($package in @('ModularPipelines', 'ModularPipelines.DotNet')) {
        Copy-Item -LiteralPath (Join-Path $PackageDirectory "$package.$PackageVersion.nupkg") -Destination $feed.FullName
    }

    $env:NUGET_PACKAGES = Join-Path $temporaryRoot 'packages'
    $feedPath = $feed.FullName.Replace('\', '/').Replace('"', '\"')
    $script = @"
#i "nuget: $feedPath"
#r "nuget: ModularPipelines, $PackageVersion"
$declarations
let runExample () =
$execution

// Ensure FSI loaded this checkout's packages, not a cached published version.
let packageRoot = System.Environment.GetEnvironmentVariable("NUGET_PACKAGES")
for assembly in [ typeof<Pipeline>.Assembly; typeof<IDotNet>.Assembly ] do
    if not (assembly.Location.StartsWith(packageRoot, System.StringComparison.OrdinalIgnoreCase)) then
        failwithf "Unexpected package assembly: %s" assembly.Location
printfn "F# Interactive documentation compiled successfully."
"@
    $scriptPath = Join-Path $temporaryRoot 'example.fsx'
    Set-Content -LiteralPath $scriptPath -Value $script -Encoding utf8
    # A child PowerShell keeps the guard's exit from bypassing this script's cleanup.
    $guardPath = (Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1').Replace("'", "''")
    $escapedScriptPath = $scriptPath.Replace("'", "''")
    & pwsh -NoProfile -Command "& '$guardPath' -TimeoutSeconds 300 -DotNetArguments @('fsi', '--exec', '$escapedScriptPath')"
    if ($LASTEXITCODE -ne 0) {
        throw "F# Interactive validation failed with exit code $LASTEXITCODE."
    }
}
finally {
    $env:NUGET_PACKAGES = $previousPackages
    $resolvedTemporaryRoot = [IO.Path]::GetFullPath($temporaryRoot)
    $tempParent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([IO.Path]::DirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
    if (-not $resolvedTemporaryRoot.StartsWith($tempParent, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove a directory outside the temporary directory: $resolvedTemporaryRoot"
    }
    if (Test-Path -LiteralPath $resolvedTemporaryRoot) {
        Remove-Item -LiteralPath $resolvedTemporaryRoot -Recurse -Force
    }
}
