[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateScript({ Test-Path -LiteralPath $_ -PathType Leaf })]
    [string] $GeneratorAssemblyPath,

    [switch] $Fix
)

$ErrorActionPreference = 'Stop'

$resolvedAssemblyPath = (Resolve-Path -LiteralPath $GeneratorAssemblyPath).Path
$expectedVersion = (
    [Reflection.AssemblyName]::GetAssemblyName($resolvedAssemblyPath)
).Version.ToString(3)
$repositoryRoot = (& git rev-parse --show-toplevel).Trim()
if ($LASTEXITCODE -ne 0 -or -not $repositoryRoot) {
    throw 'Could not resolve the repository root.'
}

$stampDefinitions = @(
    [pscustomobject]@{
        SearchText = 'GeneratedCode("ModularPipelines.OptionsGenerator"'
        Pattern = [regex]::new(
            '\[GeneratedCode\("ModularPipelines\.OptionsGenerator", "(?<Version>[^"]*)"\)\]')
        Replacement = "[GeneratedCode(`"ModularPipelines.OptionsGenerator`", `"$expectedVersion`")]"
    }
    [pscustomobject]@{
        SearchText = 'AssemblyMetadata("ModularPipelines.OptionsGenerator.Version"'
        Pattern = [regex]::new(
            '\[assembly: AssemblyMetadata\("ModularPipelines\.OptionsGenerator\.Version", "(?<Version>[^"]*)"\)\]')
        Replacement = "[assembly: AssemblyMetadata(`"ModularPipelines.OptionsGenerator.Version`", `"$expectedVersion`")]"
    }
)
$matchingLines = @(
    foreach ($stampDefinition in $stampDefinitions) {
        & git -C $repositoryRoot grep -n -F $stampDefinition.SearchText -- 'src'
        if ($LASTEXITCODE -notin @(0, 1)) {
            throw "Failed to inspect tracked generated files for '$($stampDefinition.SearchText)'."
        }
    }
)

$stamps = foreach ($line in $matchingLines) {
    if ($line -notmatch '^(?<Path>.+?):(?<Line>\d+):(?<Text>.*)$') {
        throw "Could not parse git grep output: $line"
    }

    $stamp = $stampDefinitions |
        ForEach-Object { $_.Pattern.Match($Matches.Text) } |
        Where-Object Success |
        Select-Object -First 1
    if ($null -eq $stamp) {
        throw "Could not parse generator version stamp: $line"
    }

    [pscustomobject]@{
        Path = $Matches.Path
        Line = [int] $Matches.Line
        Version = $stamp.Groups['Version'].Value
    }
}

if ($stamps.Count -eq 0) {
    throw 'No ModularPipelines.OptionsGenerator version stamps were found.'
}

$staleStamps = @($stamps | Where-Object Version -NE $expectedVersion)
if ($staleStamps.Count -eq 0) {
    Write-Host "Verified $($stamps.Count) generated version stamps at $expectedVersion."
    return
}

if (!$Fix) {
    $summary = $staleStamps |
        Group-Object Version |
        Sort-Object Name |
        ForEach-Object {
            $version = if ([string]::IsNullOrEmpty($_.Name)) { '<empty>' } else { $_.Name }
            "$version=$($_.Count)"
        }
    $examples = $staleStamps |
        Select-Object -First 10 |
        ForEach-Object { "$($_.Path):$($_.Line)" }
    throw "Found $($staleStamps.Count) stale OptionsGenerator stamps; expected " +
        "$expectedVersion ($($summary -join ', ')). Examples: $($examples -join ', ')"
}

foreach ($relativePath in $staleStamps.Path | Sort-Object -Unique) {
    $path = Join-Path $repositoryRoot $relativePath
    $content = [IO.File]::ReadAllText($path)
    $updated = $content
    foreach ($stampDefinition in $stampDefinitions) {
        $updated = $stampDefinition.Pattern.Replace(
            $updated,
            $stampDefinition.Replacement)
    }

    if ($updated -eq $content) {
        throw "No version stamp was replaced in $relativePath."
    }

    [IO.File]::WriteAllText($path, $updated)
}

Write-Host "Updated $($staleStamps.Count) generated version stamps to $expectedVersion."
