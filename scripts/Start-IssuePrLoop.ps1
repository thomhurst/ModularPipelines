# Start-IssuePrLoop.ps1
# Runs the issue-pr-loop skill one unit at a time, each in a fresh agent process.
# A long-lived agent session keeps growing its context, and model latency grows with
# it. Starting a new process per unit keeps every unit's context small.
#
# The script, not the agent, does the cheap deterministic work of each unit:
#   1. Checks that the lock Redis answers. When it does not (for example Docker is
#      down), it backs off without starting an agent.
#   2. Runs the merged-worktree sweep and surveys open PRs and issues with `gh`.
#   3. Skips drafts, `needs-human` PRs, PRs whose checks are still running, and items
#      whose GitHub state is unchanged since an agent last deferred them.
#   4. Acquires the item's Redis lock, moving on to the next candidate when another
#      worker holds it.
#   5. Starts the agent on that one item, with lower reasoning effort for PRs that
#      look ready to merge.
# After the agent ends, the script releases the lock if the agent left it held. On a
# deferred, failed, or missing result it records the item's current fingerprint with
# `AgentLocks.ps1 defer`, so later units skip it until its head, checks, mergeability,
# comments, or labels change, or until -DeferSeconds pass.
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
#           [-DeferSeconds n] [-LightEffort level] [-HeavyEffort level] [-DryRun]

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

# The fingerprint covers everything that can make a deferred item worth another look.
# Survey and post-unit fingerprints use the same fields, so they compare directly.
function Get-PrFingerprint($Pr) {
    $checks = @($Pr.statusCheckRollup | ForEach-Object {
            $name = if ($_.PSObject.Properties['name'] -and $_.name) { $_.name } else { $_.context }
            '{0}={1}' -f $name, (Get-CheckState $_)
        } | Sort-Object)
    Get-Hash ((@($Pr.headRefOid, $Pr.updatedAt, $Pr.mergeable) + $checks) -join '|')
}

function Get-IssueFingerprint($Issue) {
    $labels = @($Issue.labels | ForEach-Object { $_.name } | Sort-Object)
    $assignees = @($Issue.assignees | ForEach-Object { $_.login } | Sort-Object)
    Get-Hash ((@($Issue.updatedAt, ($labels -join ','), ($assignees -join ','))) -join '|')
}

$prFields = 'number,title,headRefOid,updatedAt,mergeable,mergeStateStatus,statusCheckRollup,isDraft,labels'
$issueFields = 'number,title,updatedAt,labels,assignees'

function Get-Candidates {
    $candidates = [Collections.Generic.List[object]]::new()

    $prs = gh pr list --repo $repoSlug --author '@me' --state open --limit 500 --json $prFields | ConvertFrom-Json
    if ($LASTEXITCODE -ne 0) { throw 'gh pr list failed.' }
    $ranked = foreach ($pr in $prs) {
        if ($pr.isDraft) { continue }
        if (@($pr.labels | Where-Object name -eq 'needs-human').Count -gt 0) { continue }
        $checks = @($pr.statusCheckRollup)
        $pending = @($checks | Where-Object { (Get-CheckState $_) -in 'PENDING', 'EXPECTED', 'QUEUED', 'IN_PROGRESS', 'WAITING', 'REQUESTED', '' })
        $failed = @($checks | Where-Object { (Get-CheckState $_) -in 'FAILURE', 'ERROR', 'TIMED_OUT', 'CANCELLED', 'ACTION_REQUIRED', 'STARTUP_FAILURE' })
        $conflicting = $pr.mergeable -eq 'CONFLICTING'
        # Running checks leave nothing to do yet; the next survey picks the PR up.
        if ($pending.Count -gt 0 -and -not $conflicting) { continue }
        $rank = if ($pr.mergeStateStatus -eq 'CLEAN' -and $failed.Count -eq 0) { 0 }
        elseif ($conflicting) { 1 }
        elseif ($failed.Count -gt 0) { 2 }
        else { 3 }
        [pscustomobject]@{
            Kind        = 'pr'
            Number      = [int]$pr.number
            LockName    = "pr-$($pr.number)"
            Label       = "PR #$($pr.number)"
            Rank        = $rank
            Fingerprint = Get-PrFingerprint $pr
            # A green, clean PR usually needs only the merge gate or a disposition reply.
            Light       = $rank -eq 0
        }
    }
    foreach ($c in @($ranked | Sort-Object Rank, Number)) { $candidates.Add($c) }

    $issues = gh search issues --repo $repoSlug --state open --no-assignee --limit 200 --json "commentsCount,$issueFields" `
        -- '-label:in-progress -label:wontfix -label:duplicate -label:question -label:needs-human' | ConvertFrom-Json
    if ($LASTEXITCODE -ne 0) { throw 'gh search issues failed.' }
    # Prefer issues with less discussion; they are usually smaller and clearer.
    foreach ($issue in @($issues | Sort-Object commentsCount, number)) {
        $candidates.Add([pscustomobject]@{
                Kind        = 'issue'
                Number      = [int]$issue.number
                LockName    = "issue-$($issue.number)"
                Label       = "issue #$($issue.number)"
                Rank        = 4
                Fingerprint = Get-IssueFingerprint $issue
                Light       = $false
            })
    }
    return $candidates
}

function Get-CurrentFingerprint($Item) {
    if ($Item.Kind -eq 'pr') {
        $pr = gh pr view $Item.Number --repo $repoSlug --json $prFields | ConvertFrom-Json
        if ($LASTEXITCODE -ne 0) { return $null }
        return Get-PrFingerprint $pr
    }
    $issue = gh issue view $Item.Number --repo $repoSlug --json $issueFields | ConvertFrom-Json
    if ($LASTEXITCODE -ne 0) { return $null }
    return Get-IssueFingerprint $issue
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

function Test-Deferred($Item, [hashtable]$Deferrals) {
    return $Item.Fingerprint -and $Deferrals[$Item.LockName] -eq $Item.Fingerprint
}

# Returns the claimed item, $null when nothing is actionable, or throws when the lock
# service fails mid-survey.
function Select-Item {
    $candidates = @(Get-Candidates)
    $deferrals = Get-Deferrals $candidates
    foreach ($item in $candidates) {
        if (Test-Deferred $item $deferrals) { continue }
        $r = Invoke-Locks @('acquire', '-LockName', $item.LockName)
        if ($r.ExitCode -eq 3) { continue }
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
            return "$head The loop driver already ran the worktree sweep and the survey, and it holds lock issue-$($Item.Number) for you through `$env:$ownerVariable. Skip the sweep, the survey and the acquire. Re-check and claim issue #$($Item.Number), implement it, release the lock, and print the RESULT line."
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
        if ($IdleSeconds -gt 0) {
            Write-Host "issue-pr-loop: nothing actionable, next survey in $IdleSeconds s"
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
        # A crash says nothing about the item, so retry it sooner.
        $ttl = if ($result) { $DeferSeconds } else { [Math]::Min($DeferSeconds, 3600) }
        if ($fingerprint) {
            $null = Invoke-Locks @('defer', '-LockName', $item.LockName, '-Fingerprint', $fingerprint, '-DeferSeconds', $ttl)
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
