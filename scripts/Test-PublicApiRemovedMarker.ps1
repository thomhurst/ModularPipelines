$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot 'PublicApiRemovedMarker.ps1')

function Assert-Equal([string] $Actual, [string] $Expected, [string] $Because) {
    if (-not [string]::Equals($Actual, $Expected, [System.StringComparison]::Ordinal)) {
        throw "$Because. Expected '$Expected' but got '$Actual'."
    }
}

Assert-Equal (Get-RemovedMarkerPrefix) '*REMOVED*' 'The marker prefix is the PublicApiAnalyzers convention'
Assert-Equal (New-RemovedMarker 'Api.Retired') '*REMOVED*Api.Retired' 'New-RemovedMarker prepends the prefix'
Assert-Equal (Get-RemovedMarkerEntry '*REMOVED*Api.Retired') 'Api.Retired' 'Get-RemovedMarkerEntry strips only the prefix'
Assert-Equal (Get-RemovedMarkerEntry (New-RemovedMarker 'Api.Round -> void')) 'Api.Round -> void' 'Marker creation and parsing round-trip'

if (-not (Test-RemovedMarker '*REMOVED*Api.Retired')) {
    throw 'Test-RemovedMarker must accept a marker.'
}

foreach ($entry in @('Api.Kept', ' *REMOVED*Api.Indented', '*removed*Api.Lowercase', '#nullable enable', '')) {
    if (Test-RemovedMarker $entry) {
        throw "Test-RemovedMarker must reject '$entry'."
    }
}

$rejected = $false
try {
    Get-RemovedMarkerEntry 'Api.Kept' | Out-Null
}
catch {
    $rejected = $true
}

if (-not $rejected) {
    throw 'Get-RemovedMarkerEntry must reject entries without the marker prefix.'
}

foreach ($line in @('#nullable enable', '# header')) {
    if (-not (Test-PublicApiHeader $line) -or (Test-PublicApiEntry $line)) {
        throw "Header classification disagrees for '$line'."
    }
}

foreach ($line in @('', ' ', "`t")) {
    if ((Test-PublicApiHeader $line) -or (Test-PublicApiEntry $line)) {
        throw 'Blank lines must not be headers or API entries.'
    }
}

# ReadApiData in the pinned PublicApiAnalyzers preserves line text. An indented
# marker/header is an API line, not a normalized removal or header.
foreach ($line in @('Api.Kept', '*REMOVED*Api.Retired', ' *REMOVED*Api.Retired', ' #nullable enable')) {
    if ((Test-PublicApiHeader $line) -or -not (Test-PublicApiEntry $line)) {
        throw "API entry classification disagrees for '$line'."
    }
}

Write-Output 'PublicApiRemovedMarker and line classification tests passed.'
