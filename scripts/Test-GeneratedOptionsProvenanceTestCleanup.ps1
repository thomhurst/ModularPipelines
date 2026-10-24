$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'GeneratedOptionsProvenanceTestCleanup.ps1')

$testRoot = Join-Path ([IO.Path]::GetTempPath()) "provenance-cleanup-$([Guid]::NewGuid().ToString('N'))"
$repository = Join-Path $testRoot 'repository'
$manifest = Join-Path $testRoot 'changes.manifest'
$script:attempts = @{}
$script:delays = 0
$script:failuresRemaining = 0
$script:failureType = [IO.IOException]
$script:failurePaths = @($repository)
$script:innerException = $null
$script:providerLock = $null

function Remove-Item {
    param($LiteralPath, [switch]$Recurse, [switch]$Force, $ErrorAction)
    $script:attempts[$LiteralPath]++
    if ($LiteralPath -in $script:failurePaths -and $script:failuresRemaining -ne 0) {
        $script:failuresRemaining--
        if ($null -ne $script:innerException) {
            throw [Activator]::CreateInstance($script:failureType, 'simulated cleanup failure', $script:innerException)
        }
        throw [Activator]::CreateInstance($script:failureType, 'simulated cleanup failure')
    }
    Microsoft.PowerShell.Management\Remove-Item @PSBoundParameters
}

function Start-Sleep {
    param([int]$Milliseconds)
    if ($Milliseconds -ne 100) { throw "Unexpected retry delay: $Milliseconds" }
    $script:delays++
    if ($null -ne $script:providerLock -and $script:delays -eq 2) {
        $script:providerLock.Dispose()
        $script:providerLock = $null
    }
}

function Reset-Fixture {
    New-Item -ItemType Directory -Path (Join-Path $repository '.git/objects') -Force | Out-Null
    Set-Content -LiteralPath (Join-Path $repository '.git/objects/object') -Value 'object'
    Set-Content -LiteralPath $manifest -Value 'manifest'
    $script:attempts = @{}
    $script:delays = 0
    $script:failuresRemaining = 0
    $script:failureType = [IO.IOException]
    $script:failurePaths = @($repository)
    $script:innerException = $null
}

try {
    Reset-Fixture
    Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest)
    if ((Test-Path $repository) -or (Test-Path $manifest) -or $script:delays -ne 0) {
        throw 'Successful cleanup did not remove both artifacts immediately.'
    }
    Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest)
    if ($script:attempts[$repository] -ne 1) { throw 'Missing paths were not skipped.' }

    Reset-Fixture
    $script:failuresRemaining = 2
    Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest)
    if ($script:attempts[$repository] -ne 3 -or $script:delays -ne 2 -or
        (Test-Path $repository) -or (Test-Path $manifest)) {
        throw 'Transient I/O failures were not retried to successful cleanup.'
    }

    Reset-Fixture
    $script:failuresRemaining = 2
    $script:failureType = [UnauthorizedAccessException]
    $script:innerException = [IO.IOException]::new('underlying filesystem failure')
    Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest)
    if ($script:attempts[$repository] -ne 3 -or $script:delays -ne 2 -or
        (Test-Path $repository) -or (Test-Path $manifest)) {
        throw 'Provider-wrapped I/O failures were not retried to successful cleanup.'
    }

    if ($IsWindows) {
        Reset-Fixture
        $script:providerLock = [IO.File]::Open(
            (Join-Path $repository '.git/objects/object'), [IO.FileMode]::Open,
            [IO.FileAccess]::Read, [IO.FileShare]::None)
        try {
            # Exercise real Remove-Item provider errors; release the file after two retries.
            Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest)
            if ($script:attempts[$repository] -ne 3 -or $script:delays -ne 2 -or
                (Test-Path $repository) -or (Test-Path $manifest)) {
                throw 'Real provider file-lock errors were not retried to successful cleanup.'
            }
        }
        finally {
            if ($null -ne $script:providerLock) {
                $script:providerLock.Dispose()
                $script:providerLock = $null
            }
        }
    }

    Reset-Fixture
    $script:failuresRemaining = 10
    $cleanupError = $null
    try { Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest) }
    catch { $cleanupError = $_ }
    if ($null -eq $cleanupError -or $script:attempts[$repository] -ne 5 -or $script:delays -ne 4 -or
        -not $cleanupError.Exception.Message.Contains($repository) -or
        -not $cleanupError.Exception.Message.Contains('simulated cleanup failure') -or (Test-Path $manifest)) {
        throw 'Persistent I/O failures were not bounded, reported, or allowed to clean the manifest.'
    }

    Reset-Fixture
    $script:failurePaths = @($repository, $manifest)
    $script:failuresRemaining = 10
    $cleanupError = $null
    try { Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest) }
    catch { $cleanupError = $_ }
    if ($null -eq $cleanupError -or $script:attempts[$repository] -ne 5 -or
        $script:attempts[$manifest] -ne 5 -or $script:delays -ne 8 -or
        -not $cleanupError.Exception.Message.Contains($repository) -or
        -not $cleanupError.Exception.Message.Contains($manifest)) {
        throw 'Combined cleanup failures did not retry and report both paths.'
    }

    Reset-Fixture
    $script:failuresRemaining = 10
    $script:failureType = [UnauthorizedAccessException]
    $cleanupError = $null
    try { Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest) }
    catch { $cleanupError = $_ }
    if ($null -eq $cleanupError -or $script:attempts[$repository] -ne 1 -or $script:delays -ne 0 -or (Test-Path $manifest)) {
        throw 'A non-I/O failure was retried or prevented manifest cleanup.'
    }

    Reset-Fixture
    $script:failuresRemaining = 10
    $originalError = $null
    $observedError = $null
    $cleanupWarnings = @()
    try {
        try { throw 'original assertion failure' }
        catch { $originalError = $_; throw }
        finally {
            Remove-GeneratedOptionsProvenanceTestArtifacts -LiteralPath @($repository, $manifest) `
                -AssertionError $originalError -WarningVariable cleanupWarnings 3>$null
        }
    }
    catch { $observedError = $_ }
    if ($observedError.Exception -ne $originalError.Exception -or $cleanupWarnings.Count -ne 1 -or
        -not $cleanupWarnings[0].Message.Contains($repository) -or (Test-Path $manifest)) {
        throw 'Cleanup replaced the assertion failure or omitted its cleanup warning.'
    }

    Write-Host 'Generated options provenance cleanup tests passed.'
}
finally {
    # Only the unique fixture root created above may be recursively removed.
    if ([IO.Path]::GetFullPath($testRoot).StartsWith(
        [IO.Path]::GetFullPath([IO.Path]::GetTempPath()), [StringComparison]::OrdinalIgnoreCase)) {
        Microsoft.PowerShell.Management\Remove-Item -LiteralPath $testRoot -Recurse -Force -ErrorAction SilentlyContinue
    }
}
