$ErrorActionPreference = 'Stop'

$cleanupScript = Join-Path $PSScriptRoot 'WorktreeCleanup.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("worktree-cleanup-{0}" -f [guid]::NewGuid())
$primaryRoot = Join-Path $testRoot 'primary'
$isolatedRoot = Join-Path $testRoot 'isolated'
$aliasRoot = Join-Path $testRoot 'primary-alias'
$casePrimaryRoot = Join-Path $testRoot 'Repo'
$caseIsolatedRoot = Join-Path $testRoot 'repo'
$script:gitCalled = $false

New-Item -ItemType Directory -Path $primaryRoot | Out-Null
New-Item -ItemType Directory -Path $isolatedRoot | Out-Null
New-Item -ItemType Directory -Path (Join-Path $primaryRoot '.git') | Out-Null
Set-Content -LiteralPath (Join-Path $isolatedRoot '.git') -Value 'gitdir: /tmp/fake-worktree'
if (-not $IsWindows) {
    New-Item -ItemType Directory -Path (Join-Path $casePrimaryRoot '.git') | Out-Null
    New-Item -ItemType Directory -Path $caseIsolatedRoot | Out-Null
    Set-Content -LiteralPath (Join-Path $caseIsolatedRoot '.git') -Value 'gitdir: /tmp/fake-case-worktree'
}
if ($IsWindows) {
    New-Item -ItemType Junction -Path $aliasRoot -Target $primaryRoot | Out-Null
}
else {
    New-Item -ItemType SymbolicLink -Path $aliasRoot -Target $primaryRoot | Out-Null
}

function global:git {
    $script:gitCalled = $true
    if ($args -contains 'remove') {
        $target = $args[-1]
        if ($target -eq $aliasRoot) {
            Remove-Item -LiteralPath $aliasRoot -Force
        }
        else {
            Remove-Item -LiteralPath $target -Recurse -Force
        }
    }
}

try {
    . $cleanupScript

    if ((Get-PrNumberFromWorktreePath -Path (Join-Path $testRoot 'pr-3045-review')) -ne 3045) {
        throw 'Canonical PR worktree number was not parsed.'
    }
    if ($null -ne (Get-PrNumberFromWorktreePath -Path (Join-Path $testRoot 'issue-3045-review'))) {
        throw 'Issue worktree was incorrectly parsed as a PR worktree.'
    }
    if (-not (Test-BranchIdentifiesPrNumber -Branch 'codex/pr-3045-r3' -PrNumber 3045)) {
        throw 'PR-numbered review branch was not recognized.'
    }
    if (Test-BranchIdentifiesPrNumber -Branch 'fix/signalr-reconnect-handoff' -PrNumber 3178) {
        throw 'Unrelated branch was matched from a misleading worktree name.'
    }

    $canonicalPrRoot = Join-Path $testRoot 'primary-worktrees'
    $canonicalPrWorktree = Join-Path $canonicalPrRoot 'pr-3045-review'
    if (-not (Test-IsCanonicalPrWorktree -Path $canonicalPrWorktree -WorktreeRoot $canonicalPrRoot `
            -Branch 'codex/pr-3045-r3' -PrNumber 3045)) {
        throw 'Canonical named PR worktree identity was not recognized.'
    }
    if (-not (Test-IsCanonicalPrWorktree -Path $canonicalPrWorktree -WorktreeRoot $canonicalPrRoot `
            -Detached -PrNumber 3045)) {
        throw 'Canonical detached PR worktree identity was not recognized.'
    }
    if (Test-IsCanonicalPrWorktree -Path $canonicalPrWorktree -WorktreeRoot $canonicalPrRoot `
            -Branch 'codex/pr-9999' -PrNumber 3045) {
        throw 'Mismatched branch/path PR identity was accepted.'
    }
    if (-not (Test-IsDescendantPath -Path (Join-Path $primaryRoot '.claude/worktrees/test') -Parent $primaryRoot)) {
        throw 'Harness-managed descendant worktree was not recognized.'
    }

    $explicitSelection = Select-MergeCleanupWorktree `
        -ValidatedWorktree $isolatedRoot `
        -CurrentBranchWorktree $null `
        -WasExplicit `
        -IdentityValid
    if ($explicitSelection -ne $isolatedRoot) {
        throw 'Explicit worktree override was not preserved for cleanup.'
    }

    $replacementRoot = Join-Path $testRoot 'replacement'
    $replacementSelection = Select-MergeCleanupWorktree `
        -ValidatedWorktree $isolatedRoot `
        -CurrentBranchWorktree $replacementRoot `
        -IdentityValid
    if ($null -ne $replacementSelection) {
        throw 'A replacement worktree was selected for cleanup.'
    }

    $invalidIdentitySelection = Select-MergeCleanupWorktree `
        -ValidatedWorktree $isolatedRoot `
        -CurrentBranchWorktree $isolatedRoot
    if ($null -ne $invalidIdentitySelection) {
        throw 'A worktree with a changed identity was selected for cleanup.'
    }

    $primarySpelling = if ($IsWindows) { ($primaryRoot + '\').ToUpperInvariant() } else { $primaryRoot + '/' }
    $output = Remove-MergedWorktree -Repo $primaryRoot -Worktree $primarySpelling -Label '#test' 6>&1

    if ($script:gitCalled) {
        throw 'Primary-checkout protection ran after a git operation.'
    }

    if (-not (Test-Path -LiteralPath $primaryRoot)) {
        throw 'Primary checkout was removed.'
    }

    if (($output -join "`n") -notmatch 'primary checkout') {
        throw 'Cleanup must explain why the primary checkout was preserved.'
    }

    $script:gitCalled = $false
    Remove-MergedWorktree -Repo $primaryRoot -Worktree $aliasRoot -Label '#alias'

    if ($script:gitCalled) {
        throw 'A primary-checkout alias reached a git operation.'
    }

    if (-not (Test-Path -LiteralPath $primaryRoot)) {
        throw 'Primary checkout was removed through an alias.'
    }

    Remove-MergedWorktree -Repo $primaryRoot -Worktree $isolatedRoot -Label '#test'

    if (-not $script:gitCalled) {
        throw 'Normal isolated-worktree cleanup did not run git.'
    }

    if (Test-Path -LiteralPath $isolatedRoot) {
        throw 'Normal isolated worktree was not removed.'
    }

    if (-not $IsWindows) {
        $script:gitCalled = $false
        Remove-MergedWorktree -Repo $casePrimaryRoot -Worktree $caseIsolatedRoot -Label '#case'

        if (-not $script:gitCalled -or (Test-Path -LiteralPath $caseIsolatedRoot)) {
            throw 'Case-distinct isolated worktree was incorrectly preserved.'
        }

        if (-not (Test-Path -LiteralPath $casePrimaryRoot)) {
            throw 'Case-distinct primary checkout was removed.'
        }
    }
}
finally {
    Remove-Item -LiteralPath Function:\git -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $aliasRoot -Force -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $testRoot -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Output 'OK primary checkout cleanup guard passed.'
