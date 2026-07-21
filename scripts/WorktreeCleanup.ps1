# WorktreeCleanup.ps1
# Shared worktree-removal helper, dot-sourced by Merge-Pr.ps1 and
# Remove-MergedWorktrees.ps1. Not meant to be run directly.
#
# Removal policy (one place, both callers):
#   - PRESERVE a worktree with uncommitted TRACKED changes (real WIP). Never
#     force-discard it; the caller logs and moves on.
#   - CLEAR untracked build artifacts (node_modules/bin/obj) — they are not work.
#   - Long-path safe: git's own delete now works because core.longpaths=true is set
#     system-wide; the `\\?\` extended-length Remove-Item is kept as a fallback for
#     environments where that config is missing.

function Remove-MergedWorktree {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Repo,       # a checkout that is NOT the one being removed (main)
        [Parameter(Mandatory)][string]$Worktree,   # path to remove
        [string]$Label = ''                        # e.g. "#1234" for log lines
    )

    if (-not (Test-Path -LiteralPath $Worktree)) {
        git -C $Repo worktree prune
        return
    }

    # Preserve genuine uncommitted work (tracked changes only — untracked is artifacts).
    $tracked = git -C $Worktree status --porcelain --untracked-files=no 2>$null
    if ($tracked) {
        Write-Host "Preserving dirty worktree $Label : $Worktree (uncommitted tracked changes)"
        return
    }

    # Primary path: let git remove it (force clears untracked artifacts; tracked is clean).
    git -C $Repo worktree remove --force $Worktree 2>$null

    # Fallback for long-path failures (only if core.longpaths is somehow off).
    if (Test-Path -LiteralPath $Worktree) {
        # Avoid recursing through a package-manager junction if one exists in a docs
        # worktree. Leave it for manual cleanup instead of risking deletion outside
        # the worktree.
        $junction = Get-ChildItem -LiteralPath $Worktree -Directory -Recurse -Force -Filter node_modules -ErrorAction SilentlyContinue |
            Where-Object { ($_.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0 } |
            Select-Object -First 1
        if ($junction) {
            Write-Host "WARNING: worktree $Label requires manual removal -- detach the node_modules junction at $($junction.FullName) first, then re-run cleanup: $Worktree"
            return
        }
        # \\?\ disables Win32 path normalization, so forward slashes (git's output
        # format) are NOT translated — convert to backslashes or the delete no-ops.
        Remove-Item -LiteralPath ('\\?\' + ($Worktree -replace '/', '\')) -Recurse -Force -ErrorAction SilentlyContinue
    }

    git -C $Repo worktree prune
    if (Test-Path -LiteralPath $Worktree) {
        Write-Host "WARNING: could not fully remove $Worktree"
    } else {
        Write-Host "Removed worktree $Label : $Worktree"
    }
}
