# Regression tests for agent evidence retention. Requires only PowerShell 7.
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'WorktreeCleanup.ps1')
$tempBase = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$testRoot = [IO.Path]::GetFullPath((Join-Path $tempBase "evidence-cleanup-$([Guid]::NewGuid().ToString('N'))"))
if (-not $testRoot.StartsWith($tempBase, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe fixture path.' }

function Assert-True([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

function New-Evidence([string]$Path, [int]$AgeDays) {
    New-Item -ItemType Directory -Path (Split-Path $Path -Parent) -Force | Out-Null
    Set-Content -LiteralPath $Path -Value 'evidence'
    (Get-Item -LiteralPath $Path).LastWriteTimeUtc = [DateTime]::UtcNow.AddDays(-$AgeDays)
}

try {
    $root = Get-AgentEvidenceRoot -MainCheckout (Join-Path $testRoot 'Repo')
    Assert-True ($root -ceq (($testRoot -replace '\\', '/') + '/Repo-evidence')) "Unexpected evidence root: $root"
    Assert-True ((Get-AgentEvidenceRoot -MainCheckout 'C:/git/Repo/') -ceq 'C:/git/Repo-evidence') 'Trailing separator leaked into the evidence root.'

    New-Evidence (Join-Path $root 'issue-1/build.log') 10
    New-Evidence (Join-Path $root 'issue-2/old.log') 10
    New-Evidence (Join-Path $root 'issue-2/nested/fresh.log') 1
    New-Evidence (Join-Path $root 'pr-3-body.md') 10
    New-Evidence (Join-Path $root 'pr-4-body.md') 1
    foreach ($directory in Get-ChildItem -LiteralPath $root -Directory -Recurse) {
        $directory.LastWriteTimeUtc = [DateTime]::UtcNow.AddDays(-10)
    }

    $wouldRemove = Remove-StaleAgentEvidence -Root $root -RetentionDays 7 -WhatIf
    Assert-True ($wouldRemove -eq 2) "Dry run counted $wouldRemove stale entries."
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'issue-1')) 'Dry run removed evidence.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-3-body.md')) 'Dry run removed an evidence file.'

    $removed = Remove-StaleAgentEvidence -Root $root -RetentionDays 7
    Assert-True ($removed -eq 2) "Removed $removed stale entries."
    Assert-True (-not (Test-Path -LiteralPath (Join-Path $root 'issue-1'))) 'Stale evidence folder survived.'
    Assert-True (-not (Test-Path -LiteralPath (Join-Path $root 'pr-3-body.md'))) 'Stale evidence file survived.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'issue-2/old.log')) 'Folder with recent writes was removed.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-4-body.md')) 'Recent evidence file was removed.'

    if ($IsWindows) {
        # A stale junction is detached, never followed: content outside the root survives.
        $outside = Join-Path $testRoot 'outside'
        New-Evidence (Join-Path $outside 'keep.txt') 10
        New-Item -ItemType Junction -Path (Join-Path $root 'linked') -Target $outside | Out-Null
        $stale = Join-Path $root 'issue-5'
        New-Evidence (Join-Path $stale 'old.log') 10
        $link = New-Item -ItemType Junction -Path (Join-Path $stale 'tools') -Target $outside
        $link.LastWriteTimeUtc = [DateTime]::UtcNow.AddDays(-10)
        (Get-Item -LiteralPath $stale).LastWriteTimeUtc = [DateTime]::UtcNow.AddDays(-10)
        $null = Remove-StaleAgentEvidence -Root $root -RetentionDays 7
        Assert-True (-not (Test-Path -LiteralPath $stale)) 'Stale evidence folder containing a junction survived.'
        Assert-True (Test-Path -LiteralPath (Join-Path $root 'linked')) 'Top-level evidence link was removed.'
        Assert-True (Test-Path -LiteralPath (Join-Path $outside 'keep.txt')) 'Evidence cleanup deleted content through a junction.'
    }

    Assert-True ((Remove-StaleAgentEvidence -Root (Join-Path $testRoot 'missing') -RetentionDays 7) -eq 0) 'Missing root reported removals.'

    # Finished evidence: closed items without a worktree go, whatever their age.
    $locks = Join-Path $testRoot 'AgentLocks.ps1'
    Set-Content -LiteralPath $locks -Value @'
param($Verb, [string]$LockName, [string]$OwnerId)
if ($LockName -eq 'pr-13') { if ($Verb -eq 'status') { 'HELD'; exit 0 }; exit 3 }
if ($Verb -eq 'status') { 'FREE' }
exit 0
'@
    foreach ($name in @('pr-10', 'issue-11', 'pr-12', 'pr-13', 'pr-14', 'pr-15', 'main-ci')) {
        New-Evidence (Join-Path $root "$name/build.log") 0
    }
    # pr-10 is also past the retention window, so a dry run must not count it twice.
    New-Evidence (Join-Path $root 'pr-10/build.log') 10
    (Get-Item -LiteralPath (Join-Path $root 'pr-10')).LastWriteTimeUtc = [DateTime]::UtcNow.AddDays(-10)
    $checked = [Collections.Generic.List[string]]::new()
    $closed = { param($LockName)
        $checked.Add($LockName)
        if ($LockName -eq 'pr-15') { throw 'state unavailable' }
        return $LockName -notin @('pr-12', 'issue-2')
    }.GetNewClosure()
    $finishedArguments = @{ Root = $root; AgentLocks = $locks; IsClosed = $closed; ActiveLockNames = @('pr-14', $null) }

    $wouldRemove = Remove-FinishedAgentEvidence @finishedArguments -WhatIf
    Assert-True (($wouldRemove -join ',') -ceq 'issue-11,pr-10') "Dry run listed finished entries: $wouldRemove."
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-10')) 'Dry run removed finished evidence.'
    Assert-True ($checked -notcontains 'pr-14' -and $checked -notcontains 'main-ci') 'State was read for an active or non-item entry.'
    $staleAfterFinished = Remove-StaleAgentEvidence -Root $root -RetentionDays 7 -Skip $wouldRemove -WhatIf
    Assert-True ($staleAfterFinished -eq 0) "Dry run counted $staleAfterFinished finished entries again as stale."

    $removed = Remove-FinishedAgentEvidence @finishedArguments
    Assert-True (($removed -join ',') -ceq 'issue-11,pr-10') "Removed finished entries: $removed."
    Assert-True (-not (Test-Path -LiteralPath (Join-Path $root 'pr-10'))) 'Closed PR evidence survived.'
    Assert-True (-not (Test-Path -LiteralPath (Join-Path $root 'issue-11'))) 'Closed issue evidence survived.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-12')) 'Open item evidence was removed.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'issue-2')) 'Open issue evidence was removed.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-13')) 'Evidence of a held item was removed.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-14')) 'Evidence of an item with a worktree was removed.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-15')) 'Evidence with unreadable state was removed.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'main-ci')) 'Non-item evidence was removed as finished.'
    Assert-True ((Remove-FinishedAgentEvidence -Root $root -AgentLocks (Join-Path $testRoot 'missing.ps1') -IsClosed { $true }).Count -eq 0) 'Missing lock script allowed removals.'
    Assert-True (Test-Path -LiteralPath (Join-Path $root 'pr-12')) 'Missing lock script removed evidence.'
    Write-Host 'OK agent evidence cleanup tests passed.'
}
finally {
    if (Test-Path -LiteralPath $testRoot) { Remove-Item -LiteralPath $testRoot -Recurse -Force }
}
