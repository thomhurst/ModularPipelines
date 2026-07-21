# Regression tests for per-worktree AgentLocks markers.

$ErrorActionPreference = 'Stop'

$agentLocks = Join-Path $PSScriptRoot 'AgentLocks.ps1'
$suffix = [Guid]::NewGuid().ToString('N')
$ownerId = "agent-lock-marker-tests-$suffix"
$issueNumber = Get-Random -Minimum 100000 -Maximum 999999
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) "modularpipelines-agent-lock-tests-$suffix"
$repo = Join-Path $testRoot 'repo'
$issueWorktree = Join-Path $testRoot 'issue'
$worktreeA = Join-Path $testRoot 'marker-a'
$worktreeB = Join-Path $testRoot 'marker-b'

$issueLock = "issue-$issueNumber"
$lockA = "pr-agentlocks-a-$suffix"
$lockB = "pr-agentlocks-b-$suffix"
$explicitLock = "pr-agentlocks-explicit-$suffix"
$staleLock = "pr-agentlocks-stale-$suffix"
$allLocks = @($issueLock, $lockA, $lockB, $explicitLock)

function Invoke-AgentLocks {
    param(
        [Parameter(Mandatory)]
        [string]$WorkingDirectory,

        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    Push-Location $WorkingDirectory
    try {
        $output = @(& pwsh -NoProfile -File $agentLocks @Arguments -OwnerId $ownerId 2>&1)
        $exitCode = $LASTEXITCODE
    }
    finally {
        Pop-Location
    }

    [pscustomobject]@{
        ExitCode = $exitCode
        Output = (($output | ForEach-Object { $_.ToString() }) -join "`n").Trim()
    }
}

function Assert-ExitZero {
    param(
        [Parameter(Mandatory)]
        $Result,

        [Parameter(Mandatory)]
        [string]$Operation
    )

    if ($Result.ExitCode -ne 0) {
        throw "$Operation failed with exit $($Result.ExitCode): $($Result.Output)"
    }
}

function Assert-Status {
    param(
        [Parameter(Mandatory)]
        [string]$WorkingDirectory,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [Parameter(Mandatory)]
        [string]$Expected,

        [Parameter(Mandatory)]
        [string]$Operation
    )

    $result = Invoke-AgentLocks -WorkingDirectory $WorkingDirectory -Arguments $Arguments
    Assert-ExitZero -Result $result -Operation $Operation
    if ($result.Output -ne $Expected) {
        throw "$Operation returned '$($result.Output)', expected '$Expected'"
    }
}

function Invoke-Git {
    param([Parameter(ValueFromRemainingArguments)] [string[]]$Arguments)

    & git @Arguments 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "git command failed: git $($Arguments -join ' ')"
    }
}

try {
    New-Item -ItemType Directory -Force $repo | Out-Null
    Invoke-Git init --initial-branch=main $repo
    Invoke-Git -C $repo config user.email 'agent-lock-tests@example.invalid'
    Invoke-Git -C $repo config user.name 'Agent Lock Tests'
    Set-Content -LiteralPath (Join-Path $repo 'fixture.txt') -Value 'fixture'
    Invoke-Git -C $repo add fixture.txt
    Invoke-Git -C $repo commit -m 'test fixture'
    Invoke-Git -C $repo worktree add -b "issue-$issueNumber-marker-test" $issueWorktree HEAD
    Invoke-Git -C $repo worktree add -b marker-a $worktreeA HEAD
    Invoke-Git -C $repo worktree add -b marker-b $worktreeB HEAD

    # Simulate marker left by the pre-fix implementation in shared local config.
    Invoke-Git -C $repo config --local agent.lockName $staleLock

    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('acquire', '-LockName', $issueLock)) -Operation 'acquire issue lock'
    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('acquire', '-LockName', $lockA, '-Worktree', $worktreeA)) -Operation 'acquire marker A'
    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('acquire', '-LockName', $lockB)) -Operation 'acquire marker B'
    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('renew', '-LockName', $lockB, '-Worktree', $worktreeB)) -Operation 'write marker B during renew'

    # Leave B's marker behind while making its lock FREE. An issue worktree must
    # derive its issue lock instead of reading B's marker from shared config.
    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('release', '-LockName', $lockB)) -Operation 'release marker B explicitly'
    Assert-Status $issueWorktree @('status') 'HELD-BY-ME' 'implicit issue status'
    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('acquire', '-LockName', $lockB)) -Operation 'reacquire marker B'
    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('renew', '-LockName', $lockB, '-Worktree', $worktreeB)) -Operation 'rewrite marker B during renew'
    Assert-ExitZero -Result (Invoke-AgentLocks $issueWorktree @('release')) -Operation 'implicit issue release'
    Assert-Status $repo @('status', '-LockName', $issueLock) 'FREE' 'explicit issue status after implicit release'
    Assert-Status $repo @('status', '-LockName', $lockB) 'HELD-BY-ME' 'marker B remains held'

    # Markers in linked worktrees must resolve independently for implicit verbs.
    Assert-Status $worktreeA @('status') 'HELD-BY-ME' 'implicit marker A status'
    Assert-Status $worktreeB @('status') 'HELD-BY-ME' 'implicit marker B status'
    Assert-ExitZero -Result (Invoke-AgentLocks $worktreeA @('release')) -Operation 'implicit marker A release'
    Assert-Status $repo @('status', '-LockName', $lockA) 'FREE' 'marker A released'
    Assert-Status $repo @('status', '-LockName', $lockB) 'HELD-BY-ME' 'marker B isolated from marker A release'
    Assert-ExitZero -Result (Invoke-AgentLocks $worktreeB @('release')) -Operation 'implicit marker B release'

    # Explicit names always override any marker in the current worktree.
    Assert-ExitZero -Result (Invoke-AgentLocks $repo @('acquire', '-LockName', $explicitLock)) -Operation 'acquire explicit lock'
    Assert-Status $worktreeA @('status', '-LockName', $explicitLock) 'HELD-BY-ME' 'explicit status override'
    Assert-ExitZero -Result (Invoke-AgentLocks $worktreeB @('release', '-LockName', $explicitLock)) -Operation 'explicit release override'
    Assert-Status $repo @('status', '-LockName', $explicitLock) 'FREE' 'explicit lock released'

    Write-Host 'OK AgentLocks worktree marker tests passed.'
}
finally {
    if (Test-Path -LiteralPath $repo) {
        foreach ($lockName in $allLocks) {
            $status = Invoke-AgentLocks $repo @('status', '-LockName', $lockName)
            if ($status.ExitCode -eq 0 -and $status.Output -eq 'HELD-BY-ME') {
                Invoke-AgentLocks $repo @('release', '-LockName', $lockName) | Out-Null
            }
        }
    }

    $tempRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
    $resolvedTestRoot = [System.IO.Path]::GetFullPath($testRoot)
    if (-not $resolvedTestRoot.StartsWith($tempRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove test directory outside temp: $resolvedTestRoot"
    }

    Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force -ErrorAction SilentlyContinue
}
