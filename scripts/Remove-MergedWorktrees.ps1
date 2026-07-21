# Remove-MergedWorktrees.ps1
# Safety-net sweep for the issue-pr-loop skill: removes every worktree whose branch
# has already merged, regardless of who merged it (this loop, another agent, a human).
#
# SQUASH-SAFE DETECTION — this is the whole point:
#   A squash (or rebase) merge rewrites history, so the branch tip is NOT an ancestor
#   of main and `git merge-base --is-ancestor` reports "not merged". Ancestry is never
#   used as a merge signal for branches. GitHub's own records are, in four tiers:
#     1. merged-PR head branch name  (cheap, bulk: one `gh pr list` call)
#     2. merged-PR head tip SHA      (same call: catches detached/renamed checkouts
#                                     sitting exactly on a merged PR's tip)
#     3. detached HEAD reachable from origin/main (local, free: a detached checkout of
#                                     an old main commit — A/B baselines etc. Applied to
#                                     DETACHED worktrees only; a branch is declared
#                                     intent and needs positive PR evidence)
#     4. commit→PR association       (`gh api repos/../commits/<sha>/pulls`, one call
#                                     per still-unmatched worktree: survives squash
#                                     merges, branch deletion, AND the 1000-PR list
#                                     window aging out)
#
#   DELIBERATELY NOT a signal: a [gone] upstream branch. [gone] only means the remote
#   ref was deleted — which also happens when a PR is CLOSED UNMERGED and its branch
#   pruned. Treating [gone] as "merged" wrongly reaps unmerged work (verified: a closed-
#   unmerged PR's worktree got flagged). Merged-PR state is the only trustworthy signal.
#
# ORPHANED DIRECTORIES — the other half of the pile-up:
#   A failed `worktree remove` followed by `worktree prune` leaves a directory whose
#   .git file points at a gitdir that no longer exists. Such dirs are invisible to
#   `git worktree list`, so the sweep also scans the directories where worktrees are
#   known to live and reaps any dir that provably WAS a worktree of this repo
#   (its .git file targets $mainRepo/.git/worktrees/* and that gitdir is gone).
#   Dirs without a .git worktree marker (artifacts, scratch output) are never touched.
#
# Guards (never delete work):
#   - skip the main checkout and anything inside it (.claude/worktrees is harness-managed)
#   - skip locked worktrees (an agent session may still own them)
#   - skip a branch/tip that has an OPEN PR (branch reused for active work)
#   - PRESERVE any worktree with uncommitted tracked changes (shared helper)
#   - worktrees with NO merge evidence are kept and listed; opt in to reaping old
#     clean ones with -StaleDays <n>
#
# Run it once per loop iteration (cheap: ~3 gh calls in bulk, plus one association
# call per unmatched leftover — a set that shrinks to near-zero after the first run).
#
# Usage:  pwsh scripts/Remove-MergedWorktrees.ps1 [-Repo owner/name] [-WhatIf] [-StaleDays n]
# Exit:   0 always (a sweep failure must not break the loop; problems are logged)

[CmdletBinding()]
param(
    [string]$Repo,
    [switch]$WhatIf,
    # Opt-in: also remove CLEAN worktrees with no PR evidence at all (scratch / A-B
    # checkouts, local-only fix branches) whose HEAD commit is older than this many
    # days. 0 = off. Dirty worktrees are still preserved.
    [int]$StaleDays = 0
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'WorktreeCleanup.ps1')
$repoArgs = @(); if ($Repo) { $repoArgs = @('--repo', $Repo) }

function Warn([string]$m) { [Console]::Error.WriteLine("sweep: $m") }

# "Exit 0 always" is load-bearing: a sweep failure must never kill an otherwise-healthy
# loop iteration. $ErrorActionPreference is 'Stop', so any unguarded throw (a git call
# failing, an unexpected gh output shape, a null string op) would exit non-zero. Wrap the
# whole body so every such error is logged and swallowed.
try {
    # Authoritative merge signal: merged-PR head branches AND head tip SHAs (squash-safe).
    # --limit 1000 covers any realistic leftover window for the NAME tier; anything older
    # falls through to the per-commit association tier below.
    $mergedNames = @{}; $mergedOids = @{}; $openNames = @{}; $openOids = @{}
    $rawMerged = gh pr list @repoArgs --state merged --limit 1000 --json headRefName,headRefOid 2>$null
    if ($LASTEXITCODE -ne 0) { Warn "could not list merged PRs (exit $LASTEXITCODE) -- skipping sweep this round"; exit 0 }
    foreach ($p in (($rawMerged -join "`n") | ConvertFrom-Json)) {
        if ($p.headRefName) { $mergedNames[$p.headRefName.Trim()] = $true }
        if ($p.headRefOid) { $mergedOids[$p.headRefOid.Trim()] = $true }
    }

    # Open-PR head branches/tips: never remove a worktree that is actively in review.
    $rawOpen = gh pr list @repoArgs --state open --limit 1000 --json headRefName,headRefOid 2>$null
    if ($LASTEXITCODE -eq 0) {
        foreach ($p in (($rawOpen -join "`n") | ConvertFrom-Json)) {
            if ($p.headRefName) { $openNames[$p.headRefName.Trim()] = $true }
            if ($p.headRefOid) { $openOids[$p.headRefOid.Trim()] = $true }
        }
    }

    # Repo slug for the per-commit association API (gh api takes no --repo flag).
    $slug = $Repo
    if (-not $slug) {
        $slug = gh repo view --json nameWithOwner --jq .nameWithOwner 2>$null
        if ($LASTEXITCODE -ne 0) { $slug = $null }
    }

    $mainRepo = ((git worktree list --porcelain) | Where-Object { $_ -like 'worktree *' } |
        Select-Object -First 1) -replace '^worktree ', ''

    # Refresh origin/main for the detached-reachability tier. Best-effort: a stale
    # origin/main only makes that tier MISS (safe direction), never over-delete.
    git -C $mainRepo fetch origin main --quiet 2>$null
    $mainTip = git -C $mainRepo rev-parse --verify --quiet origin/main 2>$null
    if ($LASTEXITCODE -ne 0) { $mainTip = $null }

    # Walk worktrees (porcelain: worktree / branch|detached / locked records).
    $wts = @(); $cur = $null; $branch = $null; $detached = $false; $locked = $false
    foreach ($line in (git -C $mainRepo worktree list --porcelain)) {
        if ($line -like 'worktree *') { $cur = $line.Substring(9); $branch = $null; $detached = $false; $locked = $false }
        elseif ($line -like 'branch *') { $branch = ($line.Substring(7) -replace '^refs/heads/', '') }
        elseif ($line -eq 'detached') { $detached = $true }
        elseif ($line -eq 'locked' -or $line -like 'locked *') { $locked = $true }
        elseif ($line -eq '') { if ($cur) { $wts += [pscustomobject]@{ Path = $cur; Branch = $branch; Detached = $detached; Locked = $locked } }; $cur = $null }
    }
    if ($cur) { $wts += [pscustomobject]@{ Path = $cur; Branch = $branch; Detached = $detached; Locked = $locked } }

    $removed = 0; $unmatched = @()
    $nowEpoch = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
    foreach ($w in $wts) {
        if ($w.Path -eq $mainRepo) { continue }
        if ($w.Locked) { Write-Host "sweep: skipping locked worktree (session may own it): $($w.Path)"; continue }
        if ($w.Branch -and $openNames.ContainsKey($w.Branch)) { continue }   # active open PR — keep

        # Tier 1: worktree still sits on the merged PR's head branch.
        $why = $null
        if ($w.Branch -and $mergedNames.ContainsKey($w.Branch)) { $why = "merged PR head branch '$($w.Branch)'" }

        $sha = $null
        if (-not $why) {
            $sha = git -C $w.Path rev-parse HEAD 2>$null
            if ($LASTEXITCODE -ne 0) { $sha = $null }
            if ($sha -and $openOids.ContainsKey($sha)) { continue }          # tip of an open PR — keep

            # Tier 2: detached or renamed checkout sitting exactly on a merged PR's tip.
            if ($sha -and $mergedOids.ContainsKey($sha)) { $why = 'merged PR head tip SHA' }
        }

        # Tier 3 (detached only): checkout of a commit already reachable from main —
        # A/B baselines and gate parents. A named branch never qualifies here; it needs
        # positive PR evidence so a freshly-cut work branch is never reaped.
        if (-not $why -and $sha -and $w.Detached -and $mainTip) {
            git -C $mainRepo merge-base --is-ancestor $sha $mainTip 2>$null
            if ($LASTEXITCODE -eq 0) { $why = 'detached HEAD reachable from origin/main' }
        }

        # Tier 4: GitHub's commit→PR association. Survives squash merges, branch
        # deletion, and the 1000-PR list window. One API call, only for leftovers.
        if (-not $why -and $sha -and $slug) {
            $assocRaw = gh api "repos/$slug/commits/$sha/pulls" 2>$null
            if ($LASTEXITCODE -eq 0 -and $assocRaw) {
                $assoc = @(($assocRaw -join "`n") | ConvertFrom-Json)
                if (@($assoc | Where-Object { $_.state -eq 'open' }).Count -gt 0) { continue }   # commit belongs to an open PR — keep
                if (@($assoc | Where-Object { $_.merged_at }).Count -gt 0) { $why = 'merged PR via commit association' }
            }
        }

        # Opt-in stale tier: no PR evidence anywhere (never-pushed scratch). Only age
        # can justify removal, and only when the caller asked for it.
        if (-not $why -and $StaleDays -gt 0 -and $sha) {
            $commitEpoch = git -C $w.Path log -1 --format=%ct 2>$null
            if ($LASTEXITCODE -eq 0 -and $commitEpoch -and (($nowEpoch - [long]$commitEpoch) -gt ($StaleDays * 86400L))) {
                $why = "no PR evidence, HEAD commit older than $StaleDays day(s)"
            }
        }

        if (-not $why) { $unmatched += $w; continue }

        if ($WhatIf) { Write-Host "sweep: WOULD remove $($w.Path) -- $why"; continue }
        Remove-MergedWorktree -Repo $mainRepo -Worktree $w.Path -Label "($why)"
        if (-not (Test-Path -LiteralPath $w.Path)) {
            $removed++
            # Once the PR is merged the local branch has served its purpose; drop it so
            # `git branch` does not pile up alongside the worktrees. -D because a squash
            # merge leaves the tip unreachable from main by design. Never done for the
            # stale tier (no merge evidence).
            if ($w.Branch -and $why -like 'merged PR*') { git -C $mainRepo branch -D $w.Branch 2>$null }
        }
    }

    if ($unmatched.Count -gt 0) {
        Write-Host "sweep: keeping $($unmatched.Count) worktree(s) with no merge evidence:"
        foreach ($w in $unmatched) {
            $label = if ($w.Branch) { "[$($w.Branch)]" } else { '(detached)' }
            Write-Host "sweep:   $($w.Path) $label"
        }
        if ($StaleDays -eq 0) { Write-Host 'sweep: re-run with -StaleDays <n> to also remove clean ones older than n days.' }
    }

    # --- Orphaned directories: registration gone, directory left behind. -------------
    # Scan the sibling `<main>-worktrees` convention plus every parent directory of a
    # registered worktree outside the main checkout (never inside it — .claude/worktrees
    # is harness-managed).
    $registered = @{}
    foreach ($w in $wts) { $registered[(($w.Path -replace '\\', '/').TrimEnd('/')).ToLowerInvariant()] = $true }
    $mainNorm = ($mainRepo -replace '\\', '/').TrimEnd('/')
    $roots = @{}
    $roots[("$mainNorm-worktrees").ToLowerInvariant()] = "$mainNorm-worktrees"
    foreach ($w in $wts) {
        $p = ($w.Path -replace '\\', '/').TrimEnd('/')
        if ($p -eq $mainNorm -or $p.StartsWith("$mainNorm/", [System.StringComparison]::OrdinalIgnoreCase)) { continue }
        $parent = (Split-Path -Path $p -Parent) -replace '\\', '/'
        if ($parent) { $roots[$parent.ToLowerInvariant()] = $parent }
    }

    $orphansRemoved = 0
    foreach ($root in $roots.Values) {
        if (-not (Test-Path -LiteralPath $root)) { continue }
        foreach ($dir in (Get-ChildItem -LiteralPath $root -Directory -Force -ErrorAction SilentlyContinue)) {
            $norm = (($dir.FullName -replace '\\', '/').TrimEnd('/')).ToLowerInvariant()
            if ($registered.ContainsKey($norm)) { continue }
            $marker = Join-Path $dir.FullName '.git'
            if (-not (Test-Path -LiteralPath $marker -PathType Leaf)) { continue }   # not a worktree (artifacts etc.) — leave alone
            $gitdir = ((Get-Content -LiteralPath $marker -TotalCount 1) -replace '^gitdir:\s*', '') -replace '\\', '/'
            # Only reap dirs that provably WERE worktrees of THIS repo and whose
            # registration is gone. A live marker (gitdir exists) is someone else's.
            if ($gitdir -notlike "$mainNorm/.git/worktrees/*") { continue }
            if (Test-Path -LiteralPath $gitdir) { continue }
            if ($WhatIf) { Write-Host "sweep: WOULD remove orphaned dir $($dir.FullName) (dangling gitdir: $gitdir)"; continue }
            Remove-Item -LiteralPath ('\\?\' + ($dir.FullName -replace '/', '\')) -Recurse -Force -ErrorAction SilentlyContinue
            if (Test-Path -LiteralPath $dir.FullName) {
                Write-Host "sweep: WARNING could not fully remove orphaned dir $($dir.FullName)"
            }
            else {
                Write-Host "sweep: removed orphaned dir $($dir.FullName)"
                $orphansRemoved++
            }
        }
    }

    git -C $mainRepo worktree prune
    Write-Host "sweep: removed $removed merged worktree(s), $orphansRemoved orphaned dir(s)."
}
catch {
    Warn "unexpected sweep error (ignored, loop continues): $_"
}
exit 0
