# Merge-Pr.ps1
# One-command, fail-closed merge for the issue-pr-loop skill:
#   1. runs the pure gate  Assert-PrGreen.ps1  (read-only; exits 0 only when green)
#   2. merges with  gh pr merge --squash  (only if the gate passed)
#   3. removes the PR's isolated worktree
#   4. deletes the merged local and remote head branches
#
# Cleanup is *part of* the merge command — an agent cannot merge and then forget
# to remove the worktree, because it is the same call. This is the durable fix for
# the worktree pile-up the prose-only rule could not guarantee.
#
# The gate stays a separate, pure predicate ON PURPOSE: Assert-PrGreen.ps1 must be
# safe to run as a check without side effects. Merge-Pr COMPOSES it; it does not
# fold merging into the assert.
#
# Worktree is resolved by matching the PR's head branch against `git worktree list`
# (robust to directory naming: pr-<N>-*, issue-<N>-*, etc.). A worktree with
# uncommitted TRACKED changes is PRESERVED, never force-discarded; untracked build
# artifacts (node_modules/bin/obj) are cleared.
#
# Usage:  pwsh scripts/Merge-Pr.ps1 -Pr 1234 [-Repo owner/name] [-Worktree <path>]
# Exit:   0 = merged (worktree removed, or preserved because dirty)
#         1 = NOT merged (gate denied, or merge failed) — nothing destroyed

[CmdletBinding()]
param(
    [Parameter(Mandatory)][int]$Pr,
    [string]$Repo,
    [string]$Worktree
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'WorktreeCleanup.ps1')
$repoArgs = @(); if ($Repo) { $repoArgs = @('--repo', $Repo) }

function Fail([string]$msg) { [Console]::Error.WriteLine("MERGE ABORTED #${Pr} -- $msg"); exit 1 }

function Find-WorktreeForBranch([string]$RepoPath, [string]$Branch) {
    $current = $null
    foreach ($line in (git -C $RepoPath worktree list --porcelain)) {
        if ($line -like 'worktree *') { $current = $line.Substring(9) }
        elseif ($line -eq "branch refs/heads/$Branch") { return $current }
    }
    return $null
}

# --- 1. Gate: the pure predicate decides. No merge unless it exits 0. -----------
& pwsh (Join-Path $PSScriptRoot 'Assert-PrGreen.ps1') -Pr $Pr @repoArgs
if ($LASTEXITCODE -ne 0) { Fail "Assert-PrGreen denied (exit $LASTEXITCODE). Not merging." }

# Resolve the head branch before merging so post-merge cleanup can remove it.
$headRef = gh pr view $Pr @repoArgs --json headRefName --jq '.headRefName' 2>$null
if ($LASTEXITCODE -ne 0 -or -not $headRef) { Fail "could not resolve head branch (exit $LASTEXITCODE)" }
$headRef = $headRef.Trim()

# Main worktree path — git removals/prunes must run from a checkout that is NOT the
# one being removed; the first `worktree list` entry is always the main checkout.
$mainRepo = ((git worktree list --porcelain) | Where-Object { $_ -like 'worktree *' } |
    Select-Object -First 1) -replace '^worktree ', ''

# Resolve cleanup before merging. If another process checked the PR branch out in the
# primary checkout, abort while the PR is still open instead of risking that checkout.
if (-not $Worktree) {
    $Worktree = Find-WorktreeForBranch -RepoPath $mainRepo -Branch $headRef
}
if ($Worktree) {
    if (-not (Test-IsLinkedWorktree -Path $Worktree)) {
        Fail "PR branch '$headRef' is not in an isolated linked worktree. Move it out of the primary checkout first."
    }
}

# --- 2. Merge. -----------------------------------------------------------------
gh pr merge $Pr @repoArgs --squash
if ($LASTEXITCODE -ne 0) { Fail "gh pr merge failed (exit $LASTEXITCODE). Worktree untouched." }
Write-Host "Merged #${Pr} ($headRef)."

# --- 3. Remove the PR's worktree. ----------------------------------------------
# Re-resolve after the merge so a worktree reused while GitHub was processing the
# merge cannot be deleted based on stale pre-merge state.
$Worktree = Find-WorktreeForBranch -RepoPath $mainRepo -Branch $headRef
if (-not $Worktree) {
    Write-Host "No isolated worktree found for branch '$headRef' (nothing to remove)."
    git -C $mainRepo worktree prune
} else {
    Remove-MergedWorktree -Repo $mainRepo -Worktree $Worktree -Label "#${Pr}"

    # A dirty worktree is intentionally preserved. Its local and remote branches are
    # also preserved so uncommitted work retains an upstream recovery point.
    if (Test-Path -LiteralPath $Worktree) {
        Write-Host "Preserving branches for worktree #${Pr}: $Worktree"
        exit 0
    }
}

# Branch cleanup happens only after the checked-out worktree is gone. Cleanup is
# best-effort: the PR is already merged, so failures must not be reported as a
# merge abort or invite a dangerous retry of the merge operation.
$remoteRef = "refs/heads/$headRef"
git -C $mainRepo ls-remote --exit-code origin $remoteRef 2>$null | Out-Null
if ($LASTEXITCODE -eq 0) {
    git -C $mainRepo push origin --delete $headRef 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "WARNING: merged #${Pr}, but could not delete remote branch '$headRef'"
    }
}

git -C $mainRepo show-ref --verify --quiet "refs/heads/$headRef"
if ($LASTEXITCODE -eq 0) {
    git -C $mainRepo branch -D -- $headRef 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "WARNING: merged #${Pr}, but could not delete local branch '$headRef'"
    }
}

exit 0
