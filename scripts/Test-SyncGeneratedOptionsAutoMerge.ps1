$ErrorActionPreference = 'Stop'
$scriptPath = Join-Path $PSScriptRoot 'Sync-GeneratedOptionsAutoMerge.ps1'
$testDirectory = Join-Path ([IO.Path]::GetTempPath()) "modularpipelines-sync-automerge-test-$([guid]::NewGuid().ToString('N'))"
$null = New-Item -ItemType Directory -Path $testDirectory
try {
    $harnessPath = Join-Path $testDirectory 'harness.ps1'
    $callsPath = Join-Path $testDirectory 'calls.jsonl'
    @'
param([string]$ScriptPath, [string]$CallsPath, [string]$View, [int]$CodexExit, [int]$ArmExit, [int]$DisarmExit)
$ErrorActionPreference = 'Stop'
function gh {
    if ($args[0] -eq 'pr' -and $args[1] -eq 'view') {
        $global:LASTEXITCODE = 0
        return $View
    }
    if ($args -notcontains '--disable-auto') { throw 'Unexpected GitHub mutation.' }
    'disarm' | Add-Content -LiteralPath $CallsPath
    $global:LASTEXITCODE = $DisarmExit
}
function pwsh {
    $fileIndex = [Array]::IndexOf($args, '-File')
    $scriptName = Split-Path $args[$fileIndex + 1] -Leaf
    if ($scriptName -eq 'Assert-CodexReviewClear.ps1') {
        if ($args -notcontains '-RequireConversationClearance') { throw 'Missing conversation clearance gate.' }
        'codex' | Add-Content -LiteralPath $CallsPath
        $global:LASTEXITCODE = $CodexExit
    }
    elseif ($scriptName -eq 'Enable-AutoMerge.ps1') {
        $headIndex = [Array]::IndexOf($args, '-ExpectedHeadSha')
        if ($headIndex -lt 0 -or $args[$headIndex + 1] -ne ('a' * 40)) { throw 'Arming must bind the inspected head.' }
        'arm' | Add-Content -LiteralPath $CallsPath
        $global:LASTEXITCODE = $ArmExit
    }
    else { throw "Unexpected script $scriptName" }
}
& $ScriptPath -Pr 7 -Repo owner/repo -DefaultBranch main
exit $LASTEXITCODE
'@ | Set-Content -LiteralPath $harnessPath

    $view = @{
        state = 'OPEN'; isDraft = $false; isCrossRepository = $false
        headRefName = 'automated/update-cli-options-git'; baseRefName = 'main'
        headRefOid = 'a' * 40; autoMergeRequest = @{ enabledAt = '2026-11-01T00:00:00Z' }
    }
    function Assert-Sync([string[]]$ExpectedCalls, [int]$CodexExit = 0, [int]$ArmExit = 0,
        [int]$DisarmExit = 0, [int]$ExpectedExit = 0) {
        Remove-Item -LiteralPath $callsPath -ErrorAction SilentlyContinue
        $output = & pwsh -NoProfile -File $harnessPath -ScriptPath $scriptPath -CallsPath $callsPath `
            -View ($view | ConvertTo-Json -Compress) -CodexExit $CodexExit -ArmExit $ArmExit -DisarmExit $DisarmExit 2>&1
        $actualExit = $LASTEXITCODE
        $calls = if (Test-Path -LiteralPath $callsPath) { @(Get-Content -LiteralPath $callsPath) } else { @() }
        if ($actualExit -ne $ExpectedExit -or ($calls -join ',') -ne ($ExpectedCalls -join ',')) {
            throw "Unexpected sync (exit $actualExit; calls $($calls -join ',')): $($output -join '`n')"
        }
    }

    Assert-Sync @('disarm', 'codex', 'arm')
    Assert-Sync @('disarm', 'codex') -CodexExit 1
    Assert-Sync @('disarm', 'codex', 'arm') -ArmExit 1
    Assert-Sync @('disarm') -DisarmExit 1 -ExpectedExit 1
    $view.isDraft = $true
    Assert-Sync @('disarm')
    $view.isDraft = $false
    $view.autoMergeRequest = $null
    Assert-Sync @('codex', 'arm')
    Assert-Sync @('codex') -CodexExit 1
    $view.headRefName = 'feature/unrelated'
    Assert-Sync @()
    $view.headRefName = 'automated/update-cli-options-git'
    $view.isCrossRepository = $true
    Assert-Sync @()
    $view.isCrossRepository = $false
    $view.baseRefName = 'release'
    Assert-Sync @()
    $view.baseRefName = 'main'
    $view.state = 'CLOSED'
    Assert-Sync @()
}
finally {
    $resolved = [IO.Path]::GetFullPath($testDirectory)
    $parent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([IO.Path]::DirectorySeparatorChar)
    if ([IO.Path]::GetDirectoryName($resolved) -ne $parent -or
        [IO.Path]::GetFileName($resolved) -notlike 'modularpipelines-sync-automerge-test-*') {
        throw 'Refusing cleanup outside the temporary sync auto-merge test directory.'
    }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
Write-Output 'OK generated options eligibility, disarm-before-check, denial, and head-bound arming passed.'
$global:LASTEXITCODE = 0
