# Shared helpers for public API baseline lines and the *REMOVED* convention.
#
# A marker in PublicAPI.Unshipped.txt retires an entry that stays in
# PublicAPI.Shipped.txt until a release ships the unshipped baseline and collapses
# the pair. Every script that reads or writes markers dot-sources this file so the
# prefix and its parsing rule live in one place.
# Keep API lines exact: ReadApiData in PublicApiAnalyzers 5.6.0 does not trim
# whitespace before recognizing markers or comparing their payloads.

function Test-PublicApiHeader([string] $Line) {
    return -not [string]::IsNullOrEmpty($Line) -and
        $Line.StartsWith('#', [System.StringComparison]::Ordinal)
}

function Test-PublicApiEntry([string] $Line) {
    return -not [string]::IsNullOrWhiteSpace($Line) -and
        -not (Test-PublicApiHeader $Line)
}

function Get-RemovedMarkerPrefix {
    return '*REMOVED*'
}

function Test-RemovedMarker([string] $Entry) {
    return $Entry.StartsWith((Get-RemovedMarkerPrefix), [System.StringComparison]::Ordinal)
}

function Get-RemovedMarkerEntry([string] $Marker) {
    if (-not (Test-RemovedMarker $Marker)) {
        throw "Entry is not a *REMOVED* marker: '$Marker'."
    }

    return $Marker.Substring((Get-RemovedMarkerPrefix).Length)
}

function New-RemovedMarker([string] $Entry) {
    return "$(Get-RemovedMarkerPrefix)$Entry"
}
