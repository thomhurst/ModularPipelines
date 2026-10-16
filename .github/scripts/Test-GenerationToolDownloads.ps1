$ErrorActionPreference = 'Stop'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) "generation-download-tests-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $testRoot | Out-Null

try {
    $fixture = Join-Path $testRoot 'fixture'
    Set-Content -LiteralPath $fixture -Value 'Unverified content must never be executed.' -NoNewline
    $expectedHash = (Get-FileHash -LiteralPath $fixture -Algorithm SHA256).Hash.ToLowerInvariant()
    $manifest = Join-Path $testRoot 'downloads.json'
    $destination = Join-Path $testRoot 'tool.tar.gz'
    $downloadState = @{ Failure = $false; Requests = 0 }

    # Simulate the network, including a request that writes bytes before failing.
    function Invoke-WebRequest {
        param($Uri, $OutFile, $TimeoutSec, $MaximumRetryCount)
        $downloadState.Requests++
        Copy-Item -LiteralPath $fixture -Destination $OutFile
        if ($downloadState.Failure) {
            throw 'Simulated network failure.'
        }
    }

    function Write-TestManifest([string] $hash, [string] $url = 'https://example.com/tool-1.0.tar.gz') {
        @{ tool = @{ version = '1.0'; url = $url; sha256 = $hash } } |
            ConvertTo-Json -Depth 3 | Set-Content -LiteralPath $manifest
    }

    function Assert-NoPartialDownload {
        if (Get-ChildItem -LiteralPath $testRoot -Filter '*.partial') {
            throw 'Partial download was not cleaned up.'
        }
    }

    function Assert-DownloadFails([string] $message) {
        $failed = $false
        try {
            & "$PSScriptRoot/Get-GenerationTool.ps1" -Tool tool -Destination $destination -ManifestPath $manifest
        }
        catch {
            if ($_.Exception.Message -notlike $message) {
                throw
            }
            $failed = $true
        }
        if (-not $failed) {
            throw "Expected failure matching '$message'."
        }
        Assert-NoPartialDownload
    }

    Write-TestManifest $expectedHash
    & "$PSScriptRoot/Get-GenerationTool.ps1" -Tool tool -Destination $destination -ManifestPath $manifest
    if ((Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash -ne $expectedHash) {
        throw 'Verified download did not reach its destination.'
    }
    Assert-NoPartialDownload

    # A failed refresh must preserve the previously verified destination.
    Set-Content -LiteralPath $fixture -Value 'Tampered content' -NoNewline
    Assert-DownloadFails 'SHA256 mismatch*'
    if ((Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash -ne $expectedHash) {
        throw 'Checksum mismatch replaced the verified destination.'
    }

    Remove-Item -LiteralPath $destination
    Assert-DownloadFails 'SHA256 mismatch*'
    if (Test-Path -LiteralPath $destination) {
        throw 'Checksum mismatch exposed an unverified destination.'
    }

    $downloadState.Failure = $true
    Assert-DownloadFails 'Simulated network failure.'
    if (Test-Path -LiteralPath $destination) {
        throw 'Network failure exposed an incomplete destination.'
    }

    $requestsBeforeInvalidPins = $downloadState.Requests
    Write-TestManifest 'invalid'
    Assert-DownloadFails 'Missing or invalid download pin*'
    Write-TestManifest $expectedHash 'http://example.com/tool-1.0.tar.gz'
    Assert-DownloadFails 'Missing or invalid download pin*'
    '{}' | Set-Content -LiteralPath $manifest
    Assert-DownloadFails 'Missing or invalid download pin*'
    if ($downloadState.Requests -ne $requestsBeforeInvalidPins) {
        throw 'Invalid pin initiated a network request.'
    }

    $pins = Get-Content -LiteralPath "$PSScriptRoot/generation-tool-downloads.json" -Raw | ConvertFrom-Json -AsHashtable
    foreach ($entry in $pins.GetEnumerator()) {
        if ($entry.Value.sha256 -cnotmatch '^[a-f0-9]{64}$' -or -not $entry.Value.version -or
            ([uri] $entry.Value.url).Scheme -ne 'https' -or
            $entry.Value.url -match '/releases/latest/|raw.githubusercontent.com/[^/]+/[^/]+/(main|master)/|/stable.txt') {
            throw "Invalid repository download pin: $($entry.Key)."
        }
    }

    Write-Host 'Generation download tests passed.'
}
finally {
    if (-not ([IO.Path]::GetFullPath($testRoot)).StartsWith([IO.Path]::GetFullPath([IO.Path]::GetTempPath()), [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Test cleanup path is outside the temporary directory.'
    }
    Remove-Item -LiteralPath $testRoot -Recurse -Force
}
