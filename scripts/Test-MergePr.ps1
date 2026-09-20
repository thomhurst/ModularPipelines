$ErrorActionPreference = 'Stop'

$scriptPath = Join-Path $PSScriptRoot 'Merge-Pr.ps1'
$script = Get-Content -LiteralPath $scriptPath -Raw

$mergeMatch = [regex]::Match($script, '(?m)^gh pr merge .*$', 'IgnoreCase')
if (-not $mergeMatch.Success) {
    throw 'Merge-Pr.ps1 must invoke gh pr merge.'
}

if ($mergeMatch.Value -match '--delete-branch') {
    throw 'gh pr merge must not delete a branch before its worktree is removed.'
}

if ($mergeMatch.Value -notmatch '--match-head-commit\s+\$headSha') {
    throw 'Merge-Pr must atomically require the commit validated by the gate.'
}

$primaryGuardIndex = $script.IndexOf("is not in an isolated linked worktree")
if ($primaryGuardIndex -lt 0 -or $primaryGuardIndex -gt $mergeMatch.Index) {
    throw 'Primary-checkout cleanup guard must run before gh pr merge.'
}

$cleanupIndex = $script.IndexOf('Remove-MergedWorktree', $mergeMatch.Index)
$remoteDeleteIndex = $script.IndexOf('push origin --delete', $mergeMatch.Index)
$localDeleteIndex = $script.IndexOf('branch -D', $mergeMatch.Index)

if ($cleanupIndex -lt 0) {
    throw 'Merged worktree cleanup is missing.'
}

if ($remoteDeleteIndex -lt 0 -or $localDeleteIndex -lt 0) {
    throw 'Post-merge remote/local branch cleanup is missing.'
}

if ($cleanupIndex -gt $remoteDeleteIndex -or $cleanupIndex -gt $localDeleteIndex) {
    throw 'The worktree must be removed before deleting its local or remote branch.'
}

$mergedConfirmationIndex = $script.IndexOf('Write-Host "Merged', $mergeMatch.Index)
if ($mergedConfirmationIndex -lt 0) {
    throw 'Successful merge confirmation is missing.'
}

$postMergeResolutionIndex = $script.IndexOf('# Re-resolve after the merge', $mergedConfirmationIndex)
if ($postMergeResolutionIndex -lt 0 -or $postMergeResolutionIndex -gt $cleanupIndex) {
    throw 'PR worktree must be re-resolved after the merge and before cleanup.'
}

$postMergeScript = $script.Substring($mergedConfirmationIndex)
if ($postMergeScript -match '(?m)^\s*Fail\s+') {
    throw 'Post-merge cleanup must warn rather than report that the merge was aborted.'
}

if ($script -notmatch '\$worktreeWasExplicit') {
    throw 'Merge-Pr must preserve whether -Worktree was explicitly supplied.'
}

if ($script -notmatch '\$worktreeIdentityToken') {
    throw 'Merge-Pr must retain an identity token for the validated worktree.'
}

$stalePruneIndex = $script.IndexOf('worktree prune')
$linkedGuardCallIndex = $script.IndexOf('Test-IsLinkedWorktree -Path $Worktree')
if ($stalePruneIndex -lt 0 -or $stalePruneIndex -gt $linkedGuardCallIndex) {
    throw 'Stale worktree registrations must be pruned before linked-worktree validation.'
}

$testDirectory = Join-Path ([IO.Path]::GetTempPath()) "modularpipelines-merge-test-$([guid]::NewGuid().ToString('N'))"
$null = New-Item -ItemType Directory -Path $testDirectory
try {
    $harnessPath = Join-Path $testDirectory 'harness.ps1'
    $logPath = Join-Path $testDirectory 'merge-arguments.json'
    @'
param([string]$MergeScript, [string]$LogPath, [string]$GateOutput)
$ErrorActionPreference = 'Stop'
function pwsh {
    if ($args -notcontains '-Json') { throw 'The merge wrapper must request the gate result.' }
    $global:LASTEXITCODE = 0
    $GateOutput
}
function git {
    if ($args[0] -eq 'worktree') { 'worktree /test-main' }
    $global:LASTEXITCODE = 0
}
function gh {
    if ($args[0] -ne 'pr' -or $args[1] -ne 'merge') { throw 'Unexpected GitHub operation.' }
    ConvertTo-Json -InputObject @($args) -Compress | Set-Content -LiteralPath $LogPath
    # Simulate GitHub rejecting a head change after the gate succeeded.
    $global:LASTEXITCODE = 1
}
& $MergeScript -Pr 123
exit $LASTEXITCODE
'@ | Set-Content -LiteralPath $harnessPath

    $validatedSha = 'a' * 40
    $gateResult = @{ headRefOid = $validatedSha; headRefName = 'test-branch' } | ConvertTo-Json -Compress
    $output = & pwsh -NoProfile -File $harnessPath -MergeScript $scriptPath -LogPath $logPath -GateOutput $gateResult 2>&1
    if ($LASTEXITCODE -ne 1 -or ($output -join "`n") -notmatch 'Worktree untouched') {
        throw 'A changed PR head must abort the merge before cleanup.'
    }
    $mergeArguments = @(Get-Content -LiteralPath $logPath -Raw | ConvertFrom-Json)
    $shaIndex = [Array]::IndexOf($mergeArguments, '--match-head-commit')
    if ($shaIndex -lt 0 -or $mergeArguments[$shaIndex + 1] -ne $validatedSha) {
        throw 'The GitHub merge must receive exactly the SHA returned by the gate.'
    }

    Remove-Item -LiteralPath $logPath
    $output = & pwsh -NoProfile -File $harnessPath -MergeScript $scriptPath -LogPath $logPath -GateOutput '{}' 2>&1
    if ($LASTEXITCODE -ne 1 -or (Test-Path -LiteralPath $logPath)) {
        throw 'A missing validated SHA must fail before any merge attempt.'
    }
}
finally {
    $resolvedTestDirectory = [IO.Path]::GetFullPath($testDirectory)
    $expectedParent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([IO.Path]::DirectorySeparatorChar)
    if ([IO.Path]::GetDirectoryName($resolvedTestDirectory) -ne $expectedParent -or
        [IO.Path]::GetFileName($resolvedTestDirectory) -notlike 'modularpipelines-merge-test-*') {
        throw 'Refusing cleanup outside the temporary merge-test directory.'
    }
    Remove-Item -LiteralPath $resolvedTestDirectory -Recurse -Force
}

Write-Output 'OK Merge-Pr ordering, validated-head binding, and failure semantics passed.'
