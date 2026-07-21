# Assert-PrGreen.ps1
# Fail-closed merge gate for the issue-pr-loop skill.
#
# Exits 0 ONLY when PR #<N> is provably safe to merge:
#   - state OPEN
#   - mergeable == MERGEABLE
#   - mergeStateStatus == CLEAN
#   - every status check is terminal AND passing (CheckRun: status=COMPLETED and
#     conclusion in SUCCESS/SKIPPED/NEUTRAL; StatusContext: state=SUCCESS)
#   - no current-head top-level review has actionable finding headings
#   - no unresolved review threads
#
# Any other state — pending/queued/in-progress checks, an empty rollup, a failed
# check, an unresolved thread, or an API hiccup — exits 1 so the caller MUST NOT
# merge. The default on uncertainty is DENY.
#
# Why this exists: the repo is on a tier WITHOUT branch protection, so GitHub
# cannot enforce "green before merge" itself. This script is the deterministic
# gate the prose rules alone cannot guarantee — the skill calls it immediately
# before `gh pr merge`, and only merges if it exits 0.
#
# Usage: pwsh scripts/Assert-PrGreen.ps1 -Pr 1234 [-Repo owner/name]
# Exit:  0 = safe to merge; 1 = DO NOT MERGE (reason printed to stderr).

[CmdletBinding()]
param(
    [Parameter(Mandatory)][int]$Pr,
    [string]$Repo
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'AssertPrGreenReviewHeuristics.ps1')

function Deny([string]$reason) {
    [Console]::Error.WriteLine("DO NOT MERGE #${Pr} -- $reason")
    exit 1
}

$repoArgs = @()
if ($Repo) { $repoArgs = @('--repo', $Repo) }

# Re-fetch fresh — survey output goes stale within seconds.
$raw = gh pr view $Pr @repoArgs --json number,state,mergeable,mergeStateStatus,statusCheckRollup,latestReviews,commits 2>$null
if ($LASTEXITCODE -ne 0) { Deny "gh pr view failed (exit $LASTEXITCODE)" }
try {
    $view = $raw | ConvertFrom-Json
}
catch {
    Deny "could not parse PR state: $($_.Exception.Message)"
}

if ($view.state -ne 'OPEN') { Deny "state=$($view.state) (need OPEN)" }
if ($view.mergeable -ne 'MERGEABLE') { Deny "mergeable=$($view.mergeable) (need MERGEABLE)" }
if ($view.mergeStateStatus -ne 'CLEAN') { Deny "mergeStateStatus=$($view.mergeStateStatus) (need CLEAN)" }

$checks = @($view.statusCheckRollup | Where-Object { $null -ne $_ })
if ($checks.Count -eq 0) {
    Deny "statusCheckRollup is empty -- no checks registered yet; refusing to merge blind"
}

$allowedConclusions = @('SUCCESS', 'SKIPPED', 'NEUTRAL')
$bad = @()
foreach ($c in $checks) {
    if ($null -ne $c.status) {
        # CheckRun: must be terminal AND passing.
        if ($c.status -ne 'COMPLETED') {
            $bad += "$($c.name): status=$($c.status)"
            continue
        }
        if ($allowedConclusions -notcontains $c.conclusion) {
            $bad += "$($c.name): conclusion=$($c.conclusion)"
        }
    }
    elseif ($null -ne $c.state) {
        # StatusContext: state must be SUCCESS.
        if ($c.state -ne 'SUCCESS') {
            $bad += "$($c.context): state=$($c.state)"
        }
    }
    else {
        $bad += "unrecognized check entry: $($c | ConvertTo-Json -Compress)"
    }
}
if ($bad.Count -gt 0) {
    Deny ("checks not green: " + ($bad -join '; '))
}

$headCommit = @($view.commits | Where-Object { $null -ne $_ }) | Select-Object -Last 1
$headCommittedAt = if ($headCommit) { ConvertTo-UtcDateTimeOffset $headCommit.committedDate } else { $null }

# Top-level review comments are not review threads, so GitHub does not expose a
# resolved flag for them. Fail closed on common review-finding shapes instead
# of treating a COMMENTED review as automatically safe. A stale Claude review
# from before the current head commit is ignored only when the current-head
# claude-review check completed green.
$latestReviews = @($view.latestReviews | Where-Object { $null -ne $_ })
$requestedChanges = @($latestReviews | Where-Object { $_.state -eq 'CHANGES_REQUESTED' })
if ($requestedChanges.Count -gt 0) {
    $authors = ($requestedChanges | ForEach-Object { $_.author.login }) -join ', '
    Deny "latest review requests changes from: $authors"
}

$actionableReviews = @()
foreach ($review in $latestReviews) {
    if ($review.state -ne 'COMMENTED') { continue }
    if (Test-StaleBotReviewCanBeIgnored -Review $review -HeadCommitCommittedAt $headCommittedAt -Checks $checks) {
        continue
    }

    $body = [string]$review.body
    $reason = Get-ActionableReviewBodyReason -Body $body
    if ($reason) {
        $actionableReviews += "$($review.author.login): $reason"
    }
}
if ($actionableReviews.Count -gt 0) {
    Deny ("latest top-level review comment has actionable finding(s): " + ($actionableReviews -join '; '))
}

# Unresolved review threads block merge even when a review's state is COMMENTED.
if ($Repo) {
    $owner, $name = $Repo -split '/', 2
}
else {
    $nwo = gh repo view --json nameWithOwner --jq '.nameWithOwner' 2>$null
    if ($LASTEXITCODE -ne 0 -or -not $nwo) { Deny "could not resolve repo for review-thread check" }
    $owner, $name = $nwo.Trim() -split '/', 2
}

$query = 'query($owner:String!,$repo:String!,$pr:Int!){repository(owner:$owner,name:$repo){pullRequest(number:$pr){reviewThreads(first:100){pageInfo{hasNextPage} nodes{isResolved}}}}}'
$threadsRaw = gh api graphql -f query=$query -F owner=$owner -F repo=$name -F pr=$Pr 2>$null
if ($LASTEXITCODE -ne 0) { Deny "could not fetch review threads (exit $LASTEXITCODE)" }
try {
    $reviewThreads = ($threadsRaw | ConvertFrom-Json).data.repository.pullRequest.reviewThreads
}
catch {
    Deny "could not parse review threads: $($_.Exception.Message)"
}
# More than one page means we cannot see every thread; deny rather than pass blind.
if ($reviewThreads.pageInfo.hasNextPage) { Deny "more than 100 review threads -- too many to inspect safely" }
$unresolved = @($reviewThreads.nodes | Where-Object { -not $_.isResolved }).Count
if ($unresolved -gt 0) { Deny "$unresolved unresolved review thread(s)" }

Write-Host "OK #${Pr} -- MERGEABLE, CLEAN, $($checks.Count) check(s) green, no unresolved threads. Safe to merge."
exit 0
