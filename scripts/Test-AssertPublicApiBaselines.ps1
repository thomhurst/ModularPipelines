$ErrorActionPreference = 'Stop'

$scriptPath = Join-Path $PSScriptRoot 'Assert-PublicApiBaselines.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("public-api-baselines-{0}" -f [guid]::NewGuid())

function Add-File([string] $RelativePath, [string] $Content = '') {
    $path = Join-Path $testRoot $RelativePath
    $directory = Split-Path $path -Parent
    New-Item -ItemType Directory -Path $directory -Force | Out-Null
    Set-Content -LiteralPath $path -Value $Content
}

function Add-BaselinePair([string] $RelativeDirectory) {
    Add-File "$RelativeDirectory/PublicAPI.Shipped.txt" '#nullable enable'
    Add-File "$RelativeDirectory/PublicAPI.Unshipped.txt" '#nullable enable'
}

function Get-AssertFailureMessage {
    # Runs the script in-process so the thrown message is inspected raw; the console
    # error view wraps long lines on Linux and would break a match on the full text.
    try {
        & $scriptPath -RepositoryRoot $testRoot | Out-Null
        return $null
    }
    catch {
        return $_.Exception.Message
    }
}

try {
    Add-File 'src/ModularPipelines/ModularPipelines.csproj' '<Project />'
    Add-BaselinePair 'src/ModularPipelines'
    Add-File 'src/ModularPipelines.Cmd/ModularPipelines.Cmd.csproj' '<Project />'
    # A marker whose entry is still shipped is the expected removal shape.
    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Shipped.txt' "#nullable enable`nApi.Retired`nApi.Kept"
    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired`nApi.Added"
    Add-File 'src/ModularPipelines.Example/ModularPipelines.Example.csproj' '<Project />'
    Add-File 'src/ModularPipelines.Example/ModularPipelines.Example.slnx' '<Solution />'
    Add-BaselinePair 'src/ModularPipelines.Example'
    Add-File 'tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj' '<Project />'
    Add-BaselinePair 'tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator'

    $successOutput = & $scriptPath -RepositoryRoot $testRoot
    if ($successOutput -notmatch 'Verified public API baselines for 4 package projects') {
        throw "Expected successful coverage output, got: $successOutput"
    }

    # Whitespace-only lines are blank, as the merge script treats them, not entries.
    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired`n `nApi.Added"
    $whitespaceLineMessage = Get-AssertFailureMessage
    if ($whitespaceLineMessage) {
        throw "Whitespace-only baseline line was treated as an entry: $whitespaceLineMessage"
    }

    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired`n*REMOVED*Api.Orphaned"
    $orphanOutput = & pwsh -NoProfile -File $scriptPath -RepositoryRoot $testRoot 2>&1
    if ($LASTEXITCODE -eq 0) {
        throw 'Orphaned *REMOVED* marker unexpectedly passed.'
    }

    if (($orphanOutput -join "`n") -notmatch 'ModularPipelines.Cmd[\\/]PublicAPI.Unshipped.txt: \*REMOVED\*Api.Orphaned') {
        throw "Orphaned marker was not reported: $($orphanOutput -join "`n")"
    }

    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired`nApi.Added`nApi.Added"
    $duplicateOutput = & pwsh -NoProfile -File $scriptPath -RepositoryRoot $testRoot 2>&1
    if ($LASTEXITCODE -eq 0) {
        throw 'Duplicate baseline entry unexpectedly passed.'
    }

    if (($duplicateOutput -join "`n") -notmatch 'ModularPipelines.Cmd[\\/]PublicAPI.Unshipped.txt: Api.Added') {
        throw "Duplicate entry was not reported: $($duplicateOutput -join "`n")"
    }

    # The same plain symbol in both files is the RS0025 shape; a shipped entry plus its
    # marker is not.
    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired`nApi.Kept"
    $pairMessage = Get-AssertFailureMessage
    if (-not $pairMessage) {
        throw 'Symbol listed in both baselines unexpectedly passed.'
    }

    if ($pairMessage -notmatch 'ModularPipelines.Cmd[\\/]PublicAPI.Unshipped.txt: Api.Kept \(also in PublicAPI.Shipped.txt\)') {
        throw "Cross-file duplicate was not reported: $pairMessage"
    }

    # Every populated category is reported in one run, not just the first one hit.
    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired`n*REMOVED*Api.Orphaned`nApi.Added`nApi.Added"
    $combinedMessage = Get-AssertFailureMessage
    if (-not $combinedMessage) {
        throw 'Orphaned marker plus duplicate entry unexpectedly passed.'
    }

    if ($combinedMessage -notmatch 'markers without a shipped entry' -or $combinedMessage -notmatch 'more than once') {
        throw "Combined failure did not report both categories: $combinedMessage"
    }

    # An entry that repeats within the file and is also shipped is one intra-file duplicate
    # plus one cross-file duplicate, not a finding per occurrence.
    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired`nApi.Kept`nApi.Kept"
    $repeatedShippedMessage = Get-AssertFailureMessage
    if (-not $repeatedShippedMessage) {
        throw 'Repeated shipped entry unexpectedly passed.'
    }

    $keptFindings = [regex]::Matches($repeatedShippedMessage, 'PublicAPI\.Unshipped\.txt: Api\.Kept').Count
    if ($keptFindings -ne 2) {
        throw "Repeated shipped entry was reported $keptFindings times instead of 2: $repeatedShippedMessage"
    }

    # Markers compare exactly, as the merge script does; trailing whitespace is an orphan.
    Add-File 'src/ModularPipelines.Cmd/PublicAPI.Unshipped.txt' "#nullable enable`n*REMOVED*Api.Retired "
    $whitespaceMessage = Get-AssertFailureMessage
    if (-not $whitespaceMessage) {
        throw 'Marker with trailing whitespace unexpectedly passed.'
    }

    if ($whitespaceMessage -notmatch 'ModularPipelines.Cmd[\\/]PublicAPI.Unshipped.txt: \*REMOVED\*Api.Retired ') {
        throw "Whitespace marker was not reported: $whitespaceMessage"
    }

    Add-BaselinePair 'src/ModularPipelines.Cmd'
    Remove-Item -LiteralPath (Join-Path $testRoot 'src/ModularPipelines.Example/PublicAPI.Unshipped.txt')
    $failureOutput = & pwsh -NoProfile -File $scriptPath -RepositoryRoot $testRoot 2>&1
    if ($LASTEXITCODE -eq 0) {
        throw 'Missing baseline unexpectedly passed.'
    }

    if (($failureOutput -join "`n") -notmatch 'ModularPipelines.Example[\\/]PublicAPI.Unshipped.txt') {
        throw "Missing baseline path was not reported: $($failureOutput -join "`n")"
    }

    # The negative cases leave the last child pwsh exit code at 1, which the Actions pwsh
    # shell would report as the step failing. Reset it rather than exit, so scripts that run
    # after this one in the same step still execute.
    $global:LASTEXITCODE = 0
    Write-Output 'Assert-PublicApiBaselines tests passed.'
}
finally {
    if (Test-Path -LiteralPath $testRoot) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
