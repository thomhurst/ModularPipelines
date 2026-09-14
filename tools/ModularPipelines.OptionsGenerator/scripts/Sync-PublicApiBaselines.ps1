[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $PackageDirectory,
    [Parameter(Mandatory)][string] $ProjectPath,
    [Parameter(Mandatory)][string] $TemporaryDirectory,
    [string[]] $ExtraBuildArguments = @(),
    [string] $DotNetExecutable = 'dotnet'
)

$ErrorActionPreference = 'Stop'

foreach ($directory in $PackageDirectory, $TemporaryDirectory) {
    if (-not (Test-Path -LiteralPath $directory -PathType Container)) {
        throw "Directory does not exist: $directory"
    }
}

if (-not (Test-Path -LiteralPath $ProjectPath -PathType Leaf)) {
    throw "Project does not exist: $ProjectPath"
}

$shipped = Join-Path $PackageDirectory 'PublicAPI.Shipped.txt'
$unshipped = Join-Path $PackageDirectory 'PublicAPI.Unshipped.txt'
foreach ($baseline in $shipped, $unshipped) {
    if (-not (Test-Path -LiteralPath $baseline -PathType Leaf)) {
        throw "Public API baseline does not exist: $baseline"
    }
}

$originalShipped = Join-Path $TemporaryDirectory 'PublicAPI.Shipped.original.txt'
$originalUnshipped = Join-Path $TemporaryDirectory 'PublicAPI.Unshipped.original.txt'
Copy-Item -LiteralPath $shipped -Destination $originalShipped -Force
Copy-Item -LiteralPath $unshipped -Destination $originalUnshipped -Force

function Invoke-PublicApiBuild(
    [string] $ErrorLogPath,
    [string] $BuildLogPath,
    [string] $FailureMessage) {
    $arguments = @(
        'build'
        $ProjectPath
        '-c'
        'Release'
        '--no-incremental'
        '--verbosity'
        'normal'
        '-p:ReportAnalyzer=true'
        '-p:TreatWarningsAsErrors=false'
        '-p:WarningsAsErrors='
        "-p:ErrorLog=$ErrorLogPath"
    ) + $ExtraBuildArguments

    Write-Host "Starting public API build: $BuildLogPath"
    $buildTimer = [Diagnostics.Stopwatch]::StartNew()
    & $DotNetExecutable @arguments *> $BuildLogPath
    $buildExitCode = $LASTEXITCODE
    $buildTimer.Stop()
    Write-Host "Public API build exited $buildExitCode after $([Math]::Round($buildTimer.Elapsed.TotalSeconds, 1)) seconds: $BuildLogPath"
    if ($buildExitCode -ne 0) {
        Get-Content -LiteralPath $BuildLogPath
        throw $FailureMessage
    }
}

$synchronized = $false
try {
    # Discover additions while the unshipped API list is empty. PublicApiAnalyzers scans
    # that list for stale siblings whenever it reports a new API, which is expensive for
    # large generated surfaces. The removal pass below declares those additions first.
    foreach ($baseline in $shipped, $unshipped) {
        $headers = @(Get-Content -LiteralPath $baseline | Where-Object {
            $_.StartsWith('#', [StringComparison]::Ordinal)
        })
        [IO.File]::WriteAllLines($baseline, $headers, [Text.UTF8Encoding]::new($false))
    }

    $errorLog = Join-Path $TemporaryDirectory 'PublicAPI.current.sarif'
    $buildLog = Join-Path $TemporaryDirectory 'PublicAPI.snapshot-build.log'
    Invoke-PublicApiBuild `
        -ErrorLogPath $errorLog `
        -BuildLogPath $buildLog `
        -FailureMessage 'Failed to create the current public API snapshot.'

    & (Join-Path $PSScriptRoot 'Write-PublicApiSnapshotFromSarif.ps1') `
        -ErrorLogPath $errorLog `
        -SnapshotPath $unshipped `
        -PackageDirectory $PackageDirectory `
        -RequireEntries

    $currentSnapshot = Join-Path $TemporaryDirectory 'PublicAPI.CurrentSnapshot.txt'
    Copy-Item -LiteralPath $unshipped -Destination $currentSnapshot -Force
    Copy-Item -LiteralPath $originalShipped -Destination $shipped -Force

    $knownEntries = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    $removalPassUnshipped = [Collections.Generic.List[string]]::new()
    foreach ($entry in Get-Content -LiteralPath $originalShipped) {
        $null = $knownEntries.Add($entry)
    }
    foreach ($entry in Get-Content -LiteralPath $originalUnshipped) {
        $null = $knownEntries.Add($entry)
        $removalPassUnshipped.Add($entry)
    }
    foreach ($entry in Get-Content -LiteralPath $currentSnapshot) {
        if ($knownEntries.Add($entry)) {
            $removalPassUnshipped.Add($entry)
        }
    }
    [IO.File]::WriteAllLines($unshipped, $removalPassUnshipped, [Text.UTF8Encoding]::new($false))

    $removalErrorLog = Join-Path $TemporaryDirectory 'PublicAPI.removals.sarif'
    $removalBuildLog = Join-Path $TemporaryDirectory 'PublicAPI.removal-build.log'
    $confirmedRemovals = Join-Path $TemporaryDirectory 'PublicAPI.ConfirmedRemovals.txt'
    Invoke-PublicApiBuild `
        -ErrorLogPath $removalErrorLog `
        -BuildLogPath $removalBuildLog `
        -FailureMessage 'Failed to collect confirmed public API removals.'

    & (Join-Path $PSScriptRoot 'Write-RemovedPublicApiSnapshotFromSarif.ps1') `
        -ErrorLogPath $removalErrorLog `
        -SnapshotPath $confirmedRemovals `
        -PackageDirectory $PackageDirectory

    & (Join-Path $PSScriptRoot 'Merge-PublicApiBaselineSnapshot.ps1') `
        -OriginalShippedPath $originalShipped `
        -OriginalUnshippedPath $originalUnshipped `
        -CurrentApiSnapshotPath $currentSnapshot `
        -ConfirmedRemovedApiPath $confirmedRemovals `
        -ShippedOutputPath $shipped `
        -UnshippedOutputPath $unshipped
    $synchronized = $true
}
finally {
    if (-not $synchronized) {
        Copy-Item -LiteralPath $originalShipped -Destination $shipped -Force
        Copy-Item -LiteralPath $originalUnshipped -Destination $unshipped -Force
    }
}
