param(
    [Parameter(Mandatory)]
    [string] $Tool,
    [Parameter(Mandatory)]
    [string] $Destination,
    [string] $ManifestPath = (Join-Path $PSScriptRoot 'generation-tool-downloads.json')
)

$ErrorActionPreference = 'Stop'
$downloads = Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json -AsHashtable
$download = $downloads[$Tool]
if (-not $download -or $download.sha256 -cnotmatch '^[a-f0-9]{64}$' -or
    ([uri] $download.url).Scheme -ne 'https' -or -not $download.version) {
    throw "Missing or invalid download pin for '$Tool'."
}

# Never expose unverified bytes at the path that subsequent install steps consume.
$destinationPath = [IO.Path]::GetFullPath($Destination)
$temporaryPath = "$destinationPath.$([guid]::NewGuid().ToString('N')).partial"
try {
    Invoke-WebRequest -Uri $download.url -OutFile $temporaryPath -TimeoutSec 120 -MaximumRetryCount 3
    $actualHash = (Get-FileHash -LiteralPath $temporaryPath -Algorithm SHA256).Hash
    if ($actualHash -ne $download.sha256) {
        throw "SHA256 mismatch for '$Tool' $($download.version)."
    }

    Move-Item -LiteralPath $temporaryPath -Destination $destinationPath -Force
}
finally {
    if (Test-Path -LiteralPath $temporaryPath) {
        Remove-Item -LiteralPath $temporaryPath -Force
    }
}
