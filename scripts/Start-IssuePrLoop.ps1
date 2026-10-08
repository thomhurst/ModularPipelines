# Start-IssuePrLoop.ps1
# Runs the issue-pr-loop skill one unit at a time, each in a fresh agent process.
# A long-lived agent session keeps growing its context, and model latency grows with
# it. Starting a new process per unit keeps every unit's context small.
#
# AGENT SELECTION (-Agent auto):
#   - CLAUDECODE=1        -> launched from Claude Code, so run `claude -p`.
#   - CODEX_THREAD_ID set -> launched from Codex, so run `codex exec`.
#   - otherwise           -> the only agent CLI on PATH; Codex when both are present.
#
# Each run gets its own lock owner (the *_AGENT_LOCK_OWNER_ID variable that
# scripts/AgentLocks.ps1 reads), so ownership never
# leaks between units. The agent ends with a `RESULT: ...` line (see the skill's
# single-unit mode). On `RESULT: queue-empty` the loop waits -IdleSeconds before the
# next survey, because pending CI and reviews make new work appear later.
#
# Run several copies in separate terminals for parallel workers; Redis locks keep
# them on different items.
#
# Stop:   create the stop file printed at startup (checked between units), or Ctrl+C.
# Usage:  pwsh scripts/Start-IssuePrLoop.ps1 [-Agent auto|claude|codex] [-IdleSeconds n] [-MaxUnits n]

[CmdletBinding()]
param(
    [ValidateSet('auto', 'claude', 'codex')]
    [string]$Agent = 'auto',
    [ValidateRange(0, 86400)]
    [int]$IdleSeconds = 600,
    # 0 means no limit.
    [ValidateRange(0, [int]::MaxValue)]
    [int]$MaxUnits = 0,
    [string]$LogDirectory = (Join-Path ([IO.Path]::GetTempPath()) 'issue-pr-loop')
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

$repo = git -C $PSScriptRoot rev-parse --show-toplevel
if ($LASTEXITCODE -ne 0 -or -not $repo) { throw 'Start-IssuePrLoop.ps1 must run from a Git checkout.' }
$repo = $repo.Trim()
$stopFile = Join-Path (Resolve-Path (git -C $repo rev-parse --git-common-dir)) 'issue-pr-loop.stop'
$selected = Resolve-Agent $Agent

# Each repository's lock script names its own owner variable (for example RESPIRE_AGENT_LOCK_OWNER_ID).
$ownerVariable = Select-String -LiteralPath (Join-Path $repo 'scripts/AgentLocks.ps1') -CaseSensitive -Pattern '\b([A-Z][A-Z0-9_]*_AGENT_LOCK_OWNER_ID)\b' |
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
$prompt = "Read $skill and follow the issue-pr-loop skill in single-unit mode."
New-Item -ItemType Directory -Force $LogDirectory | Out-Null

Write-Host "issue-pr-loop: agent=$selected repo=$repo"
Write-Host "issue-pr-loop: logs in $LogDirectory"
Write-Host "issue-pr-loop: create $stopFile to stop after the current unit"

# A nested `claude -p` refuses to start when it inherits CLAUDECODE from a parent session.
Remove-Item Env:CLAUDECODE -ErrorAction SilentlyContinue

$units = 0
while (-not (Test-Path -LiteralPath $stopFile)) {
    if ($MaxUnits -gt 0 -and $units -ge $MaxUnits) { break }
    $units++

    $runId = '{0:yyyyMMdd-HHmmss}-{1}' -f (Get-Date), ([guid]::NewGuid().ToString('N').Substring(0, 8))
    $log = Join-Path $LogDirectory "$runId.log"
    Set-Item -Path "Env:$ownerVariable" -Value "issue-pr-loop-$runId"

    Write-Host "issue-pr-loop: unit $units ($runId) started"
    if ($selected -eq 'codex') {
        codex exec --dangerously-bypass-approvals-and-sandbox -C $repo -o $log $prompt
    }
    else {
        Push-Location $repo
        try { claude -p $prompt --dangerously-skip-permissions *> $log }
        finally { Pop-Location }
    }
    $exitCode = $LASTEXITCODE

    $result = $null
    if (Test-Path -LiteralPath $log) {
        $result = Select-String -LiteralPath $log -Pattern '^\s*RESULT:\s*(.+)$' |
            Select-Object -Last 1 |
            ForEach-Object { $_.Matches[0].Groups[1].Value.Trim() }
    }
    if (-not $result) {
        # A crashed or misconfigured agent must not spin; back off before retrying.
        Write-Host "issue-pr-loop: unit $units ($runId) no RESULT line (agent exit $exitCode), see $log"
        Start-Sleep -Seconds ([Math]::Max(60, [Math]::Min($IdleSeconds, 300)))
        continue
    }
    Write-Host "issue-pr-loop: unit $units ($runId) $result"

    if ($result -eq 'queue-empty' -and $IdleSeconds -gt 0) {
        Write-Host "issue-pr-loop: queue empty, next survey in $IdleSeconds s"
        Start-Sleep -Seconds $IdleSeconds
    }
}

# Clear the stop request so the next start runs.
Remove-Item -LiteralPath $stopFile -ErrorAction SilentlyContinue
Remove-Item -Path "Env:$ownerVariable" -ErrorAction SilentlyContinue
Write-Host "issue-pr-loop: stopped after $units unit(s)"
