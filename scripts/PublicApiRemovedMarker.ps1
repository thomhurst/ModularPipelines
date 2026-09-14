# Shared helpers for the PublicApiAnalyzers *REMOVED* marker convention.
#
# A marker in PublicAPI.Unshipped.txt retires an entry that stays in
# PublicAPI.Shipped.txt until a release ships the unshipped baseline and collapses
# the pair. Every script that reads or writes markers dot-sources this file so the
# prefix and its parsing rule live in one place.

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
