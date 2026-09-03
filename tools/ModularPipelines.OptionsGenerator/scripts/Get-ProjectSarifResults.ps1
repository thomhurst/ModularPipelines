[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $ErrorLogPath,
    [Parameter(Mandatory)][string] $PackageDirectory,
    [Parameter(Mandatory)][ValidateSet('RS0016', 'RS0017')][string] $RuleId
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $ErrorLogPath -PathType Leaf)) {
    throw "Compiler error log does not exist: $ErrorLogPath"
}

if (-not (Test-Path -LiteralPath $PackageDirectory -PathType Container)) {
    throw "Package directory does not exist: $PackageDirectory"
}

$packageRoot = [IO.Path]::GetFullPath($PackageDirectory)
$pathComparison = if ($IsWindows) {
    [StringComparison]::OrdinalIgnoreCase
} else {
    [StringComparison]::Ordinal
}

function Test-IsPackagePath([string] $ArtifactUri) {
    if ([string]::IsNullOrWhiteSpace($ArtifactUri)) {
        return $false
    }

    $absoluteUri = $null
    if ([Uri]::TryCreate($ArtifactUri, [UriKind]::Absolute, [ref] $absoluteUri) -and
        $absoluteUri.IsFile) {
        $artifactPath = $absoluteUri.LocalPath
    } elseif ([IO.Path]::IsPathRooted($ArtifactUri)) {
        $artifactPath = $ArtifactUri
    } else {
        $artifactPath = Join-Path ([Environment]::CurrentDirectory) $ArtifactUri
    }

    $relativePath = [IO.Path]::GetRelativePath(
        $packageRoot,
        [IO.Path]::GetFullPath($artifactPath))
    return -not [IO.Path]::IsPathRooted($relativePath) -and
        $relativePath -ne '..' -and
        -not $relativePath.StartsWith("..$([IO.Path]::DirectorySeparatorChar)", $pathComparison) -and
        -not $relativePath.StartsWith("..$([IO.Path]::AltDirectorySeparatorChar)", $pathComparison)
}

$sarif = Get-Content -LiteralPath $ErrorLogPath -Raw | ConvertFrom-Json
$results = @($sarif.runs | ForEach-Object { @($_.results) })
foreach ($result in $results | Where-Object { $_.ruleId -eq $RuleId }) {
    $belongsToPackage = @($result.locations) | Where-Object {
        Test-IsPackagePath ([string] $_.physicalLocation.artifactLocation.uri)
    } | Select-Object -First 1

    if ($null -ne $belongsToPackage) {
        Write-Output $result
    }
}
