# Assert-CodexReviewClear.ps1
# Fail-closed check that Codex finished reviewing PR #<N> at exactly <HeadSha> with no
# unresolved findings. Assert-PrGreen.ps1 covers Claude's verdict and review threads; this
# covers the Codex summary comment, whose state marker is the only machine-readable record
# of a completed Codex review:
#
#   <!-- codex-review-state:v1 {"code_review":{"commit_sha":"...","has_unresolved_findings":false,"status":"completed"},...} -->
#
# Only the newest marker from the Codex connector counts. Any missing, malformed, pending,
# stale-head, or findings-bearing state denies.
#
# Usage: pwsh scripts/Assert-CodexReviewClear.ps1 -Pr 1234 -HeadSha <40-hex> [-Repo owner/name]
#        (-CommentsPath <file> reads a REST issue-comments JSON array instead of calling gh; for tests)
# Exit:  0 = Codex is clear for HeadSha; 1 = not clear (reason printed to stderr).

[CmdletBinding()]
param(
    [Parameter(Mandatory)][int]$Pr,
    [Parameter(Mandatory)][ValidatePattern('^[0-9a-f]{40}$')][string]$HeadSha,
    [string]$Repo,
    [string]$CommentsPath
)

$ErrorActionPreference = 'Stop'

function Deny([string]$reason) {
    [Console]::Error.WriteLine("CODEX NOT CLEAR #${Pr} -- $reason")
    exit 1
}

if ($CommentsPath) {
    $raw = Get-Content -LiteralPath $CommentsPath -Raw
}
else {
    $repoPath = if ($Repo) { "repos/$Repo" } else { 'repos/{owner}/{repo}' }
    $raw = gh api --paginate --slurp "$repoPath/issues/$Pr/comments?per_page=100" 2>$null
    if ($LASTEXITCODE -ne 0) { Deny "could not fetch PR comments (exit $LASTEXITCODE)" }
}

try {
    # --slurp wraps each page in an outer array; flatten either shape.
    $comments = @(($raw | ConvertFrom-Json -NoEnumerate) | ForEach-Object { $_ } | ForEach-Object { $_ })
}
catch {
    Deny "could not parse PR comments: $($_.Exception.Message)"
}

$marker = [regex]'codex-review-state:v1 (\{.*?\}) -->'
$state = $null
foreach ($comment in $comments | Sort-Object { [datetime]$_.updated_at }) {
    if ($comment.user.login -notmatch '^chatgpt-codex-connector(\[bot\])?$') { continue }
    $match = $marker.Match([string]$comment.body)
    if (-not $match.Success) { continue }
    try {
        $state = $match.Groups[1].Value | ConvertFrom-Json
    }
    catch {
        Deny "Codex review state is not valid JSON: $($_.Exception.Message)"
    }
}

if ($null -eq $state) { Deny 'no Codex review state found' }
$code = $state.code_review
if ($null -eq $code) { Deny 'Codex review state has no code_review entry' }
if ($code.status -ne 'completed') { Deny "Codex code review status is '$($code.status)' (need completed)" }
if ($code.commit_sha -ne $HeadSha) { Deny "Codex reviewed $($code.commit_sha), not head $HeadSha" }
if ($code.has_unresolved_findings -ne $false) { Deny 'Codex code review has unresolved findings' }

$security = $state.security_review
if ($null -ne $security -and $null -ne $security.status) {
    if ($security.status -ne 'completed') { Deny "Codex security review status is '$($security.status)'" }
    if ($security.has_unresolved_findings -ne $false) { Deny 'Codex security review has unresolved findings' }
}

Write-Output "Codex review is clear for #${Pr} at $HeadSha."
exit 0
