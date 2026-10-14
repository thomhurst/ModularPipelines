# Start-IssuePrLoop.ps1
# Runs the issue-pr-loop skill one unit at a time, each in a fresh agent process.
# A long-lived agent session keeps growing its context, and model latency grows with
# it. Starting a new process per unit keeps every unit's context small.
#
# The script, not the agent, does the cheap deterministic work of each unit:
#   1. Checks that the lock Redis answers. When it does not (for example Docker is
#      down), it backs off without starting an agent.
#   2. Runs the merged-worktree sweep and surveys open PRs and issues with `gh`.
#   3. Skips drafts, `needs-human` and `on-hold` items, PRs whose checks are still
#      running, and items whose GitHub state is unchanged since an agent last deferred
#      them. Also skips issues that cannot be worked yet: issues with open native
#      sub-issues or open `blocked_by` dependencies, issues that already have an open
#      closing PR, and bot-owned tracking issues. Ranks local `issue-*` branches with
#      unpublished commits and no open PR above new issues, then parents whose
#      sub-issues are all closed, then issues that block the most other issues.
#   4. Acquires the item's Redis lock, moving on to the next candidate when another
#      worker holds it.
#   5. Starts the agent on that one item, with lower reasoning effort for PRs that
#      look ready to merge.
# After the agent ends, the script releases the lock if the agent left it held. On a
# deferred, failed, or missing result it records the item's current fingerprint with
# `AgentLocks.ps1 defer`, so later units skip it until its head, checks, mergeability,
# comments, or labels change, or until the deferral expires. Comments marked with
# `<!-- issue-pr-loop -->` are the loop's own and do not count as a change. Each repeat
# deferral of an unchanged item doubles its expiry, starting at -DeferSeconds and capped
# at -MaxDeferSeconds.
#
# AGENT SELECTION (-Agent auto):
#   - CLAUDECODE=1        -> launched from Claude Code, so run `claude -p`.
#   - CODEX_THREAD_ID set -> launched from Codex, so run `codex exec`.
#   - otherwise           -> the only agent CLI on PATH; Codex when both are present.
#
# Each run gets its own lock owner (the *_AGENT_LOCK_OWNER_ID variable that
# scripts/AgentLocks.ps1 reads), so ownership never leaks between units. Claude units
# stream JSON events: each event goes to the unit log and each tool call prints one
# progress line, because plain `claude -p` prints nothing until the unit exits. The agent
# ends with a `RESULT: ...` line (see the skill's single-unit mode). When nothing is
# actionable the loop waits -IdleSeconds before the next survey, because pending CI and
# reviews make new work appear later.
#
# Run several copies in separate terminals for parallel workers; Redis locks keep
# them on different items.
#
# Stop:   create the stop file printed at startup (checked between units), or Ctrl+C.
# Usage:  pwsh scripts/Start-IssuePrLoop.ps1 [-Agent auto|claude|codex] [-IdleSeconds n] [-MaxUnits n]
#           [-DeferSeconds n] [-MaxDeferSeconds n] [-LightEffort level] [-HeavyEffort level] [-DryRun]

[CmdletBinding()]
param(
    [ValidateSet('auto', 'claude', 'codex')]
    [string]$Agent = 'auto',
    [ValidateRange(0, 86400)]
    [int]$IdleSeconds = 600,
    # 0 means no limit.
    [ValidateRange(0, [int]::MaxValue)]
    [int]$MaxUnits = 0,
    # How long a deferral suppresses an item whose GitHub state has not changed.
    [ValidateRange(60, 604800)]
    [int]$DeferSeconds = 21600,
    # Upper bound for a repeatedly deferred item's backoff.
    [ValidateRange(60, 604800)]
    [int]$MaxDeferSeconds = 259200,
    # Reasoning effort for PRs that look ready to merge, and for all other units.
    # An empty value keeps the agent's configured default.
    [string]$LightEffort = 'medium',
    [string]$HeavyEffort = '',
    [string]$LogDirectory = (Join-Path ([IO.Path]::GetTempPath()) 'issue-pr-loop'),
    # Survey and claim once, print the chosen item and its prompt, release, and exit.
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'

function Resolve-Agent([string]$Requested) {
    if ($Requested -ne 'auto') { return $Requested }
    if ($env:CLAUDECODE -eq '1') { return 'claude' }
    if (-not [string]::IsNullOrWhiteSpace($env:CODEX_THREAD_ID)) { return 'codex' }
    $hasCodex = [bool](Get-Command codex -ErrorAction SilentlyContinue)
    $hasClaude = [bool](Get-Command claude -ErrorAction SilentlyContinue)
    if ($hasCodex) { return 'codex' }
    if ($hasClaude) { return 'claude' }
    throw 'Neither codex nor claude is on PATH. Install one or pass -Agent.'
}

# Appends one stream-json line to the log, prints a progress line for each tool call,
# and returns the final answer text from the `result` event.
function Write-ClaudeEvent([string]$Line, [string]$Log) {
    Add-Content -LiteralPath $Log -Value $Line
    try { $streamEvent = $Line | ConvertFrom-Json -ErrorAction Stop }
    catch {
        # stderr and other non-JSON output.
        if ($Line.Trim()) { Write-Host "  $Line" }
        return
    }
    if ($streamEvent.type -eq 'result') { return [string]$streamEvent.result }
    if ($streamEvent.type -ne 'assistant') { return }
    foreach ($block in @($streamEvent.message.content)) {
        if ($block.type -ne 'tool_use') { continue }
        $toolInput = $block.input
        $detail = @($toolInput.description, $toolInput.command, $toolInput.file_path, $toolInput.pattern, $toolInput.skill, $toolInput.prompt) |
            Where-Object { $_ } |
            Select-Object -First 1
        $detail = ([string]$detail -replace '\s+', ' ').Trim()
        if ($detail.Length -gt 120) { $detail = $detail.Substring(0, 117) + '...' }
        Write-Host ('  {0:HH:mm:ss} {1}: {2}' -f (Get-Date), $block.name, $detail)
    }
}

$repo = git -C $PSScriptRoot rev-parse --show-toplevel
if ($LASTEXITCODE -ne 0 -or -not $repo) { throw 'Start-IssuePrLoop.ps1 must run from a Git checkout.' }
$repo = $repo.Trim()
$stopFile = Join-Path (Resolve-Path (git -C $repo rev-parse --git-common-dir)) 'issue-pr-loop.stop'
$selected = Resolve-Agent $Agent
$agentLocks = Join-Path $repo 'scripts/AgentLocks.ps1'
$sweepScript = Join-Path $repo 'scripts/Remove-MergedWorktrees.ps1'

# Each repository's lock script names its own owner variable (for example RESPIRE_AGENT_LOCK_OWNER_ID).
$ownerVariable = Select-String -LiteralPath $agentLocks -CaseSensitive -Pattern '\b([A-Z][A-Z0-9_]*_AGENT_LOCK_OWNER_ID)\b' |
    Select-Object -First 1 |
    ForEach-Object { $_.Matches[0].Groups[1].Value }
if (-not $ownerVariable) { throw 'scripts/AgentLocks.ps1 does not name an *_AGENT_LOCK_OWNER_ID variable.' }

# The skill lives under .claude or .agents depending on the repository; name it by path so either agent finds it.
# With core.symlinks=false a symlinked SKILL.md checks out as a one-line path stub, so require real content.
$skill = @('.agents/skills/issue-pr-loop/SKILL.md', '.claude/skills/issue-pr-loop/SKILL.md') |
    Where-Object {
        $candidate = Join-Path $repo $_
        (Test-Path -LiteralPath $candidate) -and (Select-String -LiteralPath $candidate -Pattern '^name:\s*issue-pr-loop' -Quiet)
    } |
    Select-Object -First 1
if (-not $skill) { throw 'issue-pr-loop SKILL.md not found under .claude/skills or .agents/skills.' }
New-Item -ItemType Directory -Force $LogDirectory | Out-Null

Push-Location $repo
try { $repoSlug = gh repo view --json nameWithOwner --jq .nameWithOwner }
finally { Pop-Location }
if ($LASTEXITCODE -ne 0 -or -not $repoSlug) { throw 'gh repo view failed; check gh auth status.' }
$repoSlug = $repoSlug.Trim()

function Invoke-Locks([string[]]$Arguments, [int]$TimeoutSeconds = 120) {
    # Docker Desktop can hang `docker` calls indefinitely, so bound every lock call.
    $ownerValue = [Environment]::GetEnvironmentVariable($ownerVariable)
    $job = Start-Job -ScriptBlock {
        param($Script, $LockArgs, $OwnerName, $OwnerValue)
        if ($OwnerName) { Set-Item -Path "Env:$OwnerName" -Value $OwnerValue }
        $out = @(& pwsh -NoProfile -File $Script @LockArgs 2>&1 | ForEach-Object { $_.ToString() })
        [pscustomobject]@{ ExitCode = $LASTEXITCODE; Output = ($out -join "`n").Trim() }
    } -ArgumentList $agentLocks, $Arguments, $ownerVariable, $ownerValue
    try {
        if (-not (Wait-Job $job -Timeout $TimeoutSeconds)) {
            return [pscustomobject]@{ ExitCode = 124; Output = "timed out after $TimeoutSeconds s" }
        }
        return Receive-Job $job
    }
    finally { Remove-Job $job -Force }
}

function Get-Hash([string]$Text) {
    [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($Text))).ToLowerInvariant()
}

function Get-CheckState($Check) {
    if ($Check.PSObject.Properties['conclusion'] -and $Check.conclusion) { return [string]$Check.conclusion }
    if ($Check.PSObject.Properties['state'] -and $Check.state) { return [string]$Check.state }
    return [string]$Check.status
}

# Agents mark every comment they post with this, so the loop's own blocker comments do
# not make a deferred item look changed.
$loopMarker = '<!-- issue-pr-loop -->'

# Returns the newest creation time among comments the loop did not write.
function Get-LatestOutsideComment($Comments) {
    @($Comments | Where-Object { $_ -and -not ([string]$_.body).Contains($loopMarker) } |
        ForEach-Object { [string]$_.createdAt } | Sort-Object) | Select-Object -Last 1
}

# The fingerprint covers everything that can make a deferred item worth another look.
# Survey and post-unit fingerprints use the same fields, so they compare directly.
# They leave out updatedAt, which every loop comment and cross-reference changes.
function Get-PrFingerprint($Pr) {
    $checks = @($Pr.statusCheckRollup | ForEach-Object {
            $name = if ($_.PSObject.Properties['name'] -and $_.name) { $_.name } else { $_.context }
            '{0}={1}' -f $name, (Get-CheckState $_)
        } | Sort-Object)
    $labels = @($Pr.labels | ForEach-Object { $_.name } | Sort-Object)
    # The loop replies to review threads as the PR author, which GitHub records as reviews.
    $review = @($Pr.reviews | Where-Object { $_.author.login -ne $Pr.author.login } |
        ForEach-Object { [string]$_.submittedAt } | Sort-Object) | Select-Object -Last 1
    $comment = Get-LatestOutsideComment $Pr.comments
    Get-Hash ((@($Pr.headRefOid, $Pr.mergeable, ($labels -join ','), $review, $comment) + $checks) -join '|')
}

function Get-IssueFingerprint($Issue) {
    $labels = @($Issue.labels.nodes | ForEach-Object { $_.name } | Sort-Object)
    $assignees = @($Issue.assignees.nodes | ForEach-Object { $_.login } | Sort-Object)
    $prs = @($Issue.closedByPullRequestsReferences.nodes | ForEach-Object { '{0}:{1}' -f $_.number, $_.state } | Sort-Object)
    $children = '{0}/{1}' -f $Issue.subIssuesSummary.completed, $Issue.subIssuesSummary.total
    Get-Hash ((@($Issue.lastEditedAt, ($labels -join ','), ($assignees -join ','), (Get-LatestOutsideComment $Issue.comments.nodes),
                $children, $Issue.issueDependenciesSummary.blockedBy, ($prs -join ','))) -join '|')
}

# Returns number -> state (OPEN, CLOSED, MERGED, MISSING) for issues and PRs, in one query.
# Returns $null when the lookup fails, so callers can fall back to the item's own fingerprint.
function Get-ReferenceStates([int[]]$Numbers) {
    $Numbers = @($Numbers | Sort-Object -Unique)
    $states = @{}
    if ($Numbers.Count -eq 0) { return $states }
    $owner, $name = $repoSlug -split '/', 2
    $fields = ($Numbers | ForEach-Object { "n${_}: issueOrPullRequest(number: $_) { ... on Issue { state } ... on PullRequest { state } }" }) -join ' '
    $query = 'query($owner: String!, $name: String!) { repository(owner: $owner, name: $name) { ' + $fields + ' } }'
    # A missing number makes gh exit 1, but stdout still carries the data for the rest.
    $json = gh api graphql -f "query=$query" -f "owner=$owner" -f "name=$name" 2>$null
    try { $repository = ($json | ConvertFrom-Json -ErrorAction Stop).data.repository }
    catch { return $null }
    if (-not $repository) { return $null }
    foreach ($n in $Numbers) {
        $node = $repository."n$n"
        $states[$n] = if ($node) { [string]$node.state } else { 'MISSING' }
    }
    return $states
}

# A deferral reason names its blockers as #N. Recording their states lets a survey retry
# the item as soon as a blocker closes or merges, even though the item itself is unchanged.
function Get-ReasonReferences([string]$Reason, [int]$Self) {
    @([regex]::Matches($Reason, '#(\d+)') | ForEach-Object { [int]$_.Groups[1].Value } |
        Where-Object { $_ -ne $Self } | Sort-Object -Unique | Select-Object -First 20)
}

function Format-References([int[]]$Numbers, [hashtable]$States) {
    ($Numbers | ForEach-Object { '{0}:{1}' -f $_, $States[$_] }) -join ','
}

$prFields = 'number,title,author,headRefOid,mergeable,mergeStateStatus,statusCheckRollup,isDraft,labels,comments,reviews'
$issueNodeFields = 'number title lastEditedAt author { login __typename } labels(first: 50) { nodes { name } } ' +
    'assignees(first: 10) { nodes { login } } comments(last: 20) { totalCount nodes { createdAt body } } ' +
    'subIssuesSummary { total completed } issueDependenciesSummary { blockedBy blocking } ' +
    'closedByPullRequestsReferences(first: 10, includeClosedPrs: false) { nodes { number state } }'

# Labels that take an issue out of the queue. `in-progress` is handled separately.
$excludedIssueLabels = @('wontfix', 'duplicate', 'question', 'needs-human', 'on-hold')
# Labels that take a PR out of the queue until a human removes them.
$excludedPrLabels = @('needs-human', 'on-hold')

# Counts why the last survey skipped items, so an idle survey can say why nothing was actionable.
$surveyStats = [ordered]@{}

function Reset-SurveyStats {
    foreach ($key in 'issues', 'issue-deferred', 'issue-locked', 'issue-excluded', 'issue-parent', 'issue-blocked', 'issue-has-pr', 'issue-claimed',
        'prs', 'pr-deferred', 'pr-locked', 'pr-pending', 'pr-excluded', 'pr-draft') {
        $surveyStats[$key] = 0
    }
}

function Format-SurveyStats {
    $s = $surveyStats
    ('issues: {0} open, {1} deferred, {2} locked, {3} excluded by label/assignee/bot, {4} with open sub-issues, {5} blocked, {6} with open PR, {7} claimed; ' +
        'PRs: {8} open, {9} deferred, {10} locked, {11} pending checks, {12} needs-human/on-hold, {13} draft') -f `
        $s['issues'], $s['issue-deferred'], $s['issue-locked'], $s['issue-excluded'], $s['issue-parent'], $s['issue-blocked'], $s['issue-has-pr'], $s['issue-claimed'],
        $s['prs'], $s['pr-deferred'], $s['pr-locked'], $s['pr-pending'], $s['pr-excluded'], $s['pr-draft']
}

function Invoke-GraphQl([string]$Query, [string[]]$Variables) {
    $owner, $name = $repoSlug -split '/', 2
    $json = gh api graphql -f "query=$Query" -f "owner=$owner" -f "name=$name" @Variables
    if ($LASTEXITCODE -ne 0) { throw 'gh api graphql failed.' }
    return ($json | ConvertFrom-Json).data.repository
}

function Get-OpenIssues {
    $query = 'query($owner: String!, $name: String!, $cursor: String) { repository(owner: $owner, name: $name) { ' +
        'issues(states: OPEN, first: 50, after: $cursor) { pageInfo { hasNextPage endCursor } nodes { ' + $issueNodeFields + ' } } } }'
    $issues = [Collections.Generic.List[object]]::new()
    $cursor = $null
    do {
        $variables = if ($cursor) { @('-f', "cursor=$cursor") } else { @() }
        $page = (Invoke-GraphQl $query $variables).issues
        foreach ($node in @($page.nodes)) { $issues.Add($node) }
        $cursor = $page.pageInfo.endCursor
    } while ($page.pageInfo.hasNextPage)
    return $issues
}

function Get-Issue([int]$Number) {
    $query = 'query($owner: String!, $name: String!, $number: Int!) { repository(owner: $owner, name: $name) { issue(number: $number) { ' +
        $issueNodeFields + ' } } }'
    return (Invoke-GraphQl $query @('-F', "number=$Number")).issue
}

function Test-HasLabel($Issue, [string[]]$Names) {
    @($Issue.labels.nodes | Where-Object { $_.name -in $Names }).Count -gt 0
}

function Test-HasOpenPr($Issue) {
    @($Issue.closedByPullRequestsReferences.nodes | Where-Object { $_.state -eq 'OPEN' }).Count -gt 0
}

# Local issue branches with commits that are not on origin/main, for open issues that have
# no open PR. A unit that could not push (for example because the lock service hung)
# leaves finished work here, and nothing else would publish it. A branch that ever had a
# PR is skipped: it was merged, split, or closed on purpose.
function Get-StrandedBranches([hashtable]$OpenIssues) {
    $byIssue = @{}
    foreach ($line in @(git -C $repo for-each-ref --format='%(refname:short) %(objectname)' 'refs/heads/issue-*' 2>$null)) {
        $branch, $sha = ([string]$line).Trim() -split ' ', 2
        if ($branch -notmatch '^issue-(\d+)-') { continue }
        $number = [int]$Matches[1]
        $issue = $OpenIssues[$number]
        if (-not $issue -or $byIssue.ContainsKey($number) -or (Test-HasOpenPr $issue)) { continue }
        # `git cherry` prefixes commits whose patch is already upstream with '-'.
        $unpublished = @(git -C $repo cherry origin/main $branch 2>$null | Where-Object { $_ -like '+*' }).Count
        if ($unpublished -eq 0) { continue }
        $prs = gh pr list --repo $repoSlug --state all --head $branch --json number --jq 'length'
        if ($LASTEXITCODE -ne 0 -or [int]$prs -gt 0) { continue }
        $byIssue[$number] = [pscustomobject]@{ Branch = $branch; Commits = $unpublished; Issue = $issue }
    }
    return $byIssue
}

function Get-StrandedFingerprint([string]$Branch, $Issue) {
    $sha = git -C $repo rev-parse --verify --quiet "refs/heads/$Branch"
    Get-Hash ('{0}|{1}' -f $sha, (Get-IssueFingerprint $Issue))
}

function New-Candidate([string]$Kind, [int]$Number, [int]$Rank, [string]$Fingerprint, [bool]$Light, [string]$Hint = '', [string]$Branch = '') {
    $lockKind = if ($Kind -eq 'pr') { 'pr' } else { 'issue' }
    $label = if ($Kind -eq 'pr') { "PR #$Number" } elseif ($Kind -eq 'stranded') { "stranded branch $Branch" } else { "issue #$Number" }
    [pscustomobject]@{
        Kind        = $Kind
        Number      = $Number
        LockName    = "$lockKind-$Number"
        Label       = $label
        Rank        = $Rank
        Fingerprint = $Fingerprint
        Light       = $Light
        Hint        = $Hint
        Branch      = $Branch
        Blocking    = 0
        Comments    = 0
    }
}

function Get-Candidates {
    $candidates = [Collections.Generic.List[object]]::new()

    $prs = gh pr list --repo $repoSlug --author '@me' --state open --limit 500 --json $prFields | ConvertFrom-Json
    if ($LASTEXITCODE -ne 0) { throw 'gh pr list failed.' }
    $surveyStats['prs'] = @($prs).Count
    $ranked = foreach ($pr in $prs) {
        if ($pr.isDraft) { $surveyStats['pr-draft']++; continue }
        if (@($pr.labels | Where-Object { $_.name -in $excludedPrLabels }).Count -gt 0) { $surveyStats['pr-excluded']++; continue }
        $checks = @($pr.statusCheckRollup)
        $pending = @($checks | Where-Object { (Get-CheckState $_) -in 'PENDING', 'EXPECTED', 'QUEUED', 'IN_PROGRESS', 'WAITING', 'REQUESTED', '' })
        $failed = @($checks | Where-Object { (Get-CheckState $_) -in 'FAILURE', 'ERROR', 'TIMED_OUT', 'CANCELLED', 'ACTION_REQUIRED', 'STARTUP_FAILURE' })
        $conflicting = $pr.mergeable -eq 'CONFLICTING'
        # Running checks leave nothing to do yet; the next survey picks the PR up.
        if ($pending.Count -gt 0 -and -not $conflicting) { $surveyStats['pr-pending']++; continue }
        $rank = if ($pr.mergeStateStatus -eq 'CLEAN' -and $failed.Count -eq 0) { 0 }
        elseif ($conflicting) { 1 }
        elseif ($failed.Count -gt 0) { 2 }
        else { 3 }
        # A green, clean PR usually needs only the merge gate or a disposition reply.
        New-Candidate 'pr' ([int]$pr.number) $rank (Get-PrFingerprint $pr) ($rank -eq 0)
    }
    foreach ($c in @($ranked | Sort-Object Rank, Number)) { $candidates.Add($c) }

    $issues = @(Get-OpenIssues)
    $surveyStats['issues'] = $issues.Count
    $openIssues = @{}
    foreach ($issue in $issues) { $openIssues[[int]$issue.number] = $issue }
    $stranded = Get-StrandedBranches $openIssues
    foreach ($number in @($stranded.Keys | Sort-Object)) {
        $s = $stranded[$number]
        $hint = "Local branch $($s.Branch) holds $($s.Commits) commit(s) for issue #$number that are not on origin/main, and no PR exists for it. " +
            'Follow the stranded-branch procedure in the recovery guidance: publish the finished work as a PR, or record why it cannot ship.'
        $candidates.Add((New-Candidate 'stranded' $number 4 (Get-StrandedFingerprint $s.Branch $s.Issue) $false $hint $s.Branch))
    }

    $ranked = foreach ($issue in $issues) {
        $number = [int]$issue.number
        if ($stranded.ContainsKey($number)) { continue }
        $tracking = $issue.author.__typename -eq 'Bot' -or $issue.title -eq 'Dependency Dashboard'
        if ($tracking -or @($issue.assignees.nodes).Count -gt 0 -or (Test-HasLabel $issue $excludedIssueLabels)) { $surveyStats['issue-excluded']++; continue }
        # Work on an issue with an open closing PR happens on the PR.
        if (Test-HasOpenPr $issue) { $surveyStats['issue-has-pr']++; continue }
        # A blocked issue returns to the queue on its own when its blockers close.
        if ($issue.issueDependenciesSummary.blockedBy -gt 0) { $surveyStats['issue-blocked']++; continue }
        $children = $issue.subIssuesSummary
        if ($children.total -gt $children.completed) { $surveyStats['issue-parent']++; continue }
        $fingerprint = Get-IssueFingerprint $issue
        if (Test-HasLabel $issue @('in-progress')) {
            # A claim with no open PR is stale unless a worker holds its lock; the acquire
            # in Select-Item skips it while one does.
            $surveyStats['issue-claimed']++
            $hint = "Issue #$number carries an in-progress claim, but it has no open PR and no lock holder. Look for its branch or worktree and publish that work; " +
                'otherwise remove the stale claim and implement the issue.'
            New-Candidate 'issue' $number 7 $fingerprint $false $hint
            continue
        }
        if ($children.total -gt 0) {
            $hint = "All $($children.total) native sub-issues of #$number are closed. Check the parent's remaining acceptance criteria against origin/main, " +
                'then close it with evidence or implement what is left.'
            New-Candidate 'issue' $number 5 $fingerprint $true $hint
            continue
        }
        $candidate = New-Candidate 'issue' $number 6 $fingerprint $false
        $candidate.Blocking = [int]$issue.issueDependenciesSummary.blocking
        $candidate.Comments = [int]$issue.comments.totalCount
        $candidate
    }
    # Unblock the most other issues first, then prefer less discussion: such issues are
    # usually smaller and clearer.
    foreach ($c in @($ranked | Sort-Object Rank, @{ Expression = 'Blocking'; Descending = $true }, Comments, Number)) { $candidates.Add($c) }
    return $candidates
}

function Get-CurrentFingerprint($Item) {
    try {
        switch ($Item.Kind) {
            'pr' {
                $pr = gh pr view $Item.Number --repo $repoSlug --json $prFields | ConvertFrom-Json
                if ($LASTEXITCODE -ne 0) { return $null }
                return Get-PrFingerprint $pr
            }
            'stranded' { return Get-StrandedFingerprint $Item.Branch (Get-Issue $Item.Number) }
            default { return Get-IssueFingerprint (Get-Issue $Item.Number) }
        }
    }
    catch { return $null }
}

# Returns lock name -> fingerprint for every recorded deferral among the given items.
function Get-Deferrals([object[]]$Items) {
    $deferrals = @{}
    if ($Items.Count -eq 0) { return $deferrals }
    $r = Invoke-Locks @('deferred', '-LockName', (($Items | ForEach-Object LockName) -join ','))
    if ($r.ExitCode -ne 0) { throw "deferral lookup failed (exit $($r.ExitCode)): $($r.Output)" }
    foreach ($line in ($r.Output -split "`n")) {
        $name, $fingerprint = $line.Trim() -split '=', 2
        if ($fingerprint) { $deferrals[$name] = $fingerprint }
    }
    return $deferrals
}

# A recorded deferral is `<fingerprint>` followed by optional `|key=value` fields:
# `until=<unix seconds>` ends the deferral, `n=<count>` counts consecutive deferrals of
# the same fingerprint, and `refs=<n>:<state>,...` records blocker states. The Redis
# record outlives `until`, so a repeat deferral can still read the count.
function Split-Deferral([string]$Recorded) {
    $fingerprint, $fields = $Recorded -split '\|'
    $values = @{}
    foreach ($field in @($fields)) {
        $key, $value = $field -split '=', 2
        if ($key) { $values[$key] = $value }
    }
    $refs = $values['refs']
    $numbers = @(if ($refs) { $refs -split ',' | ForEach-Object { [int]($_ -split ':', 2)[0] } })
    [pscustomobject]@{
        Fingerprint = $fingerprint
        Refs        = $refs
        Numbers     = $numbers
        Until       = if ($values['until']) { [long]$values['until'] } else { [long]::MaxValue }
        Count       = if ($values['n']) { [int]$values['n'] } else { 1 }
    }
}

function Test-Deferred($Item, [hashtable]$Deferrals, $States) {
    if (-not $Item.Fingerprint -or -not $Deferrals[$Item.LockName]) { return $false }
    $deferral = Split-Deferral $Deferrals[$Item.LockName]
    if ($deferral.Fingerprint -ne $Item.Fingerprint) { return $false }
    if ([DateTimeOffset]::UtcNow.ToUnixTimeSeconds() -ge $deferral.Until) { return $false }
    # Without fresh blocker states, fall back to the item's own fingerprint.
    if (-not $deferral.Refs -or $null -eq $States) { return $true }
    return (Format-References $deferral.Numbers $States) -eq $deferral.Refs
}

# Returns the claimed item, $null when nothing is actionable, or throws when the lock
# service fails mid-survey.
function Select-Item {
    Reset-SurveyStats
    $candidates = @(Get-Candidates)
    $deferrals = Get-Deferrals $candidates
    $states = Get-ReferenceStates @($deferrals.Values | ForEach-Object { (Split-Deferral $_).Numbers })
    foreach ($item in $candidates) {
        if (Test-Deferred $item $deferrals $states) { $surveyStats["$($item.Kind)-deferred"]++; continue }
        $r = Invoke-Locks @('acquire', '-LockName', $item.LockName)
        if ($r.ExitCode -eq 3) { $surveyStats["$($item.Kind)-locked"]++; continue }
        if ($r.ExitCode -ne 0) { throw "lock acquire for $($item.LockName) failed (exit $($r.ExitCode)): $($r.Output)" }
        return $item
    }
    return $null
}

# A preserved worktree with new source may hold unpublished work worth recovering.
function Get-RecoveryItem([string[]]$SweepOutput) {
    $lines = @($SweepOutput | Where-Object { $_ -match '^Preserving worktree .*\(new source' } | Sort-Object)
    if ($lines.Count -eq 0) { return $null }
    $item = [pscustomobject]@{
        Kind        = 'recovery'
        Number      = 0
        LockName    = 'recovery-survey'
        Label       = 'worktree recovery'
        Rank        = 5
        Fingerprint = Get-Hash ($lines -join "`n")
        Light       = $false
        Lines       = $lines
    }
    if (Test-Deferred $item (Get-Deferrals @($item))) { return $null }
    return $item
}

function Get-Prompt($Item) {
    $head = "Read $skill and follow the issue-pr-loop skill in single-unit mode for an assigned item."
    switch ($Item.Kind) {
        'pr' {
            return "$head The loop driver already ran the worktree sweep and the survey, and it holds lock pr-$($Item.Number) for you through `$env:$ownerVariable. Skip the sweep, the survey and the acquire. Work only PR #$($Item.Number), release the lock, and print the RESULT line."
        }
        'issue' {
            $hint = if ($Item.Hint) { " $($Item.Hint)" } else { '' }
            return "$head The loop driver already ran the worktree sweep and the survey, and it holds lock issue-$($Item.Number) for you through `$env:$ownerVariable. Skip the sweep, the survey and the acquire.$hint Re-check and claim issue #$($Item.Number), implement it, release the lock, and print the RESULT line."
        }
        'stranded' {
            return "$head The loop driver already ran the worktree sweep and the survey, and it holds lock issue-$($Item.Number) for you through `$env:$ownerVariable. Skip the sweep, the survey and the acquire. $($Item.Hint) Release the lock and print the RESULT line."
        }
        default {
            $list = $Item.Lines -join '; '
            return "$head The loop driver found no actionable PR or issue. The worktree sweep preserved these worktrees with new source: $list. Run the recovery procedure for the first one you can lock, release every lock, and print the RESULT line. Print RESULT: queue-empty when none needs recovery."
        }
    }
}

# Runs one agent unit and returns its exit code and final answer text.
function Invoke-Agent([string]$Prompt, [string]$Effort, [string]$Log) {
    $finalText = $null
    if ($selected -eq 'codex') {
        $effortArgs = if ($Effort) { @('-c', "model_reasoning_effort=$Effort") } else { @() }
        # Out-Host keeps Codex progress on the console instead of in this function's output.
        codex exec --dangerously-bypass-approvals-and-sandbox @effortArgs -C $repo -o $Log $Prompt | Out-Host
        $exitCode = $LASTEXITCODE
        if (Test-Path -LiteralPath $Log) { $finalText = Get-Content -LiteralPath $Log -Raw }
    }
    else {
        $effortArgs = if ($Effort) { @('--effort', $Effort) } else { @() }
        Push-Location $repo
        try {
            # The empty pipe closes stdin; otherwise claude waits 3 s for piped input.
            $finalText = @() | claude -p $Prompt --dangerously-skip-permissions @effortArgs --output-format stream-json --verbose 2>&1 |
                ForEach-Object { Write-ClaudeEvent -Line "$_" -Log $Log }
            $exitCode = $LASTEXITCODE
        }
        finally { Pop-Location }
    }
    return [pscustomobject]@{ ExitCode = $exitCode; Text = ($finalText -join "`n") }
}

function Wait-Backoff([string]$Reason) {
    $seconds = [Math]::Max(60, [Math]::Min($IdleSeconds, 300))
    Write-Host "issue-pr-loop: $Reason; retrying in $seconds s"
    Start-Sleep -Seconds $seconds
}

Write-Host "issue-pr-loop: agent=$selected repo=$repo"
Write-Host "issue-pr-loop: logs in $LogDirectory"
Write-Host "issue-pr-loop: create $stopFile to stop after the current unit"

# A nested `claude -p` refuses to start when it inherits CLAUDECODE from a parent session.
Remove-Item Env:CLAUDECODE -ErrorAction SilentlyContinue

$units = 0
while (-not (Test-Path -LiteralPath $stopFile)) {
    if ($MaxUnits -gt 0 -and $units -ge $MaxUnits) { break }

    $runId = '{0:yyyyMMdd-HHmmss}-{1}' -f (Get-Date), ([guid]::NewGuid().ToString('N').Substring(0, 8))
    $log = Join-Path $LogDirectory "$runId.log"
    Set-Item -Path "Env:$ownerVariable" -Value "issue-pr-loop-$runId"

    $ping = Invoke-Locks @('ping') -TimeoutSeconds 90
    if ($ping.ExitCode -ne 0) {
        Wait-Backoff "lock Redis unavailable (exit $($ping.ExitCode): $($ping.Output))"
        continue
    }

    $sweep = @(& pwsh -NoProfile -WorkingDirectory $repo -File $sweepScript 2>&1 | ForEach-Object { $_.ToString() })
    $sweep | Set-Content -LiteralPath (Join-Path $LogDirectory "$runId.sweep.log")

    try {
        $item = Select-Item
        if (-not $item) { $item = Get-RecoveryItem $sweep }
    }
    catch {
        Wait-Backoff "survey failed: $($_.Exception.Message)"
        continue
    }
    if (-not $item) {
        Write-Host "issue-pr-loop: nothing actionable ($(Format-SurveyStats))"
        if ($IdleSeconds -gt 0) {
            Write-Host "issue-pr-loop: next survey in $IdleSeconds s"
            Start-Sleep -Seconds $IdleSeconds
        }
        continue
    }

    if ($DryRun) {
        Write-Host "issue-pr-loop: dry run would start $($item.Label) (light=$($item.Light))"
        Write-Host (Get-Prompt $item)
        if ($item.Kind -ne 'recovery') { $null = Invoke-Locks @('release', '-LockName', $item.LockName) }
        break
    }

    $units++
    $effort = if ($item.Light) { $LightEffort } else { $HeavyEffort }
    Write-Host "issue-pr-loop: unit $units ($runId) $($item.Label)$(if ($effort) { " effort=$effort" })"
    $run = Invoke-Agent (Get-Prompt $item) $effort $log
    $exitCode = $run.ExitCode
    $result = [regex]::Matches([string]$run.Text, '(?m)^\s*RESULT:\s*(.+)$') |
        Select-Object -Last 1 |
        ForEach-Object { $_.Groups[1].Value.Trim() }

    if ($item.Kind -ne 'recovery') {
        $status = Invoke-Locks @('status', '-LockName', $item.LockName)
        if ($status.Output -eq 'HELD-BY-ME') { $null = Invoke-Locks @('release', '-LockName', $item.LockName) }
    }

    # Remember an unchanged blocker so later units do not re-examine it.
    if (-not $result -or $result -match '^(deferred|failed)\b' -or ($item.Kind -eq 'recovery' -and $result -eq 'queue-empty')) {
        $fingerprint = if ($item.Kind -eq 'recovery') { $item.Fingerprint } else { Get-CurrentFingerprint $item }
        if ($fingerprint) {
            # Each repeat deferral of an unchanged item doubles its expiry. A crash says
            # nothing about the item, so it neither counts nor waits long.
            $previous = try { (Get-Deferrals @($item))[$item.LockName] } catch { $null }
            $previous = if ($previous) { Split-Deferral $previous } else { $null }
            $count = if ($previous -and $previous.Fingerprint -eq $fingerprint) { $previous.Count } else { 0 }
            if ($result) { $count++ }
            $seconds = if ($result) { [Math]::Min($MaxDeferSeconds, $DeferSeconds * [Math]::Pow(2, [Math]::Min($count - 1, 20))) }
            else { [Math]::Min($DeferSeconds, 3600) }
            $until = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds() + [long]$seconds
            $record = '{0}|until={1}|n={2}' -f $fingerprint, $until, [Math]::Max($count, 1)
            $refs = if ($item.Kind -ne 'recovery') { Get-ReasonReferences $result $item.Number } else { @() }
            if ($refs.Count -gt 0) {
                $refStates = Get-ReferenceStates $refs
                if ($refStates) { $record += '|refs=' + (Format-References $refs $refStates) }
            }
            # The record outlives `until` so the next deferral can read the count.
            $ttl = [Math]::Min(604800, [long]$seconds + $MaxDeferSeconds)
            $null = Invoke-Locks @('defer', '-LockName', $item.LockName, '-Fingerprint', $record, '-DeferSeconds', $ttl)
            if ($result) { Write-Host ('issue-pr-loop: {0} deferred {1} time(s) unchanged; next look in {2:N1} h' -f $item.Label, $count, ($seconds / 3600)) }
        }
    }

    if (-not $result) {
        # A crashed or misconfigured agent must not spin; back off before retrying.
        Write-Host "issue-pr-loop: unit $units ($runId) no RESULT line (agent exit $exitCode), see $log"
        Wait-Backoff 'agent ended without a result'
        continue
    }
    Write-Host "issue-pr-loop: unit $units ($runId) $result"
}

# Clear the stop request so the next start runs.
Remove-Item -LiteralPath $stopFile -ErrorAction SilentlyContinue
Remove-Item -Path "Env:$ownerVariable" -ErrorAction SilentlyContinue
Write-Host "issue-pr-loop: stopped after $units unit(s)"
