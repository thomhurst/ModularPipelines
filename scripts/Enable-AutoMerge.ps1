# Enable-AutoMerge.ps1
# Arms GitHub native auto-merge (squash) for PR #<N>, pinned to the head the gate validated.
#
#   1. runs  Assert-PrGreen.ps1 -ForAutoMerge -Json  (not a draft, no conflicts, no failed
#      checks, a CLEAR Claude Code Review verdict for the current head, no actionable
#      reviews or unresolved threads) and stops on any denial
#   2. arms  gh pr merge --auto --squash --match-head-commit <validated head>
#
# Use it right after `gh pr ready`: readiness starts build/test CI, and GitHub merges once
# branch protection's required checks pass. A later push to the PR disarms auto-merge
# (the `disarm-auto-merge` job in claude-code-review.yml), so each new head needs its own
# review cycle and a new call to this script.
#
# The caller must read and disposition every review body, inline finding, and issue
# comment (including Codex's verdict) BEFORE running this; the gate cannot judge them all.
#
# Auto-merge leaves the PR's worktree in place. scripts/Remove-MergedWorktrees.ps1 cleans
# it up after the merge.
#
# Usage: pwsh scripts/Enable-AutoMerge.ps1 -Pr 1234 [-Repo owner/name]
# Exit:  0 = armed; 1 = not armed (reason printed to stderr).

[CmdletBinding()]
param(
    [Parameter(Mandatory)][int]$Pr,
    [string]$Repo
)

$ErrorActionPreference = 'Stop'

function Fail([string]$reason) {
    [Console]::Error.WriteLine("NOT ARMED #${Pr} -- $reason")
    exit 1
}

$repoArgs = @()
if ($Repo) { $repoArgs = @('--repo', $Repo) }

$gateOutput = & pwsh -NoProfile -File (Join-Path $PSScriptRoot 'Assert-PrGreen.ps1') -Pr $Pr @repoArgs -ForAutoMerge -Json
if ($LASTEXITCODE -ne 0) { Fail "auto-merge gate denied (see above)" }
try {
    $headSha = [string](($gateOutput -join "`n") | ConvertFrom-Json).headRefOid
}
catch {
    Fail "could not parse the gate result: $($_.Exception.Message)"
}
if ($headSha -notmatch '^[0-9a-f]{40}$') { Fail "the gate did not return a validated head SHA" }

# --match-head-commit makes GitHub refuse if a push moved the head after the gate ran.
gh pr merge $Pr @repoArgs --auto --squash --match-head-commit $headSha
if ($LASTEXITCODE -ne 0) { Fail "gh pr merge --auto failed (exit $LASTEXITCODE)" }

Write-Host "Armed auto-merge for #${Pr} at $headSha."
exit 0
