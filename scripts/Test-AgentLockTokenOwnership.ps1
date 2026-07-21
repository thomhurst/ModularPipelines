# Regression tests for AgentLocks token-cache ownership isolation.

$ErrorActionPreference = 'Stop'

$agentLocks = Join-Path $PSScriptRoot 'AgentLocks.ps1'
$redisContainer = 'modularpipelines-agent-locks-redis'
$suffix = [Guid]::NewGuid().ToString('N')
$issueNumber = Get-Random -Minimum 100000 -Maximum 999999
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) "modularpipelines-agent-lock-owner-tests-$suffix"
$repo = Join-Path $testRoot 'repo'
$issueWorktree = Join-Path $testRoot 'issue-worktree'
$prWorktree = Join-Path $testRoot 'pr-worktree'

$ownershipLock = "pr-agentlocks-ownership-$suffix"
$issueLock = "issue-$issueNumber"
$prWorktreeLock = "pr-agentlocks-worktree-$suffix"
$crossVersionLock = "pr-agentlocks-cross-version-$suffix"
$missingIdentityLock = "pr-agentlocks-missing-owner-$suffix"
$allLocks = @($ownershipLock, $issueLock, $prWorktreeLock, $crossVersionLock, $missingIdentityLock)

$ownerA = 'agent-lock-test-owner-a'
$ownerB = 'agent-lock-test-owner-b'
$issueOwner = 'agent-lock-test-issue-owner'
$parameterOwner = 'agent-lock-test-parameter-owner'
$crossVersionOwner = 'agent-lock-test-cross-version-owner'
$completed = $false

function Invoke-AgentLocks {
    param(
        [Parameter(Mandatory)]
        [string]$WorkingDirectory,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [AllowNull()]
        [string]$EnvironmentOwnerId = $null,

        [AllowNull()]
        [string]$CodexThreadId = $null,

        [string]$ScriptPath = $agentLocks
    )

    $previousOwnerId = [Environment]::GetEnvironmentVariable('MODULARPIPELINES_AGENT_LOCK_OWNER_ID', 'Process')
    $previousThreadId = [Environment]::GetEnvironmentVariable('CODEX_THREAD_ID', 'Process')

    try {
        [Environment]::SetEnvironmentVariable('MODULARPIPELINES_AGENT_LOCK_OWNER_ID', $EnvironmentOwnerId, 'Process')
        [Environment]::SetEnvironmentVariable('CODEX_THREAD_ID', $CodexThreadId, 'Process')

        Push-Location $WorkingDirectory
        try {
            $output = @(& pwsh -NoProfile -File $ScriptPath @Arguments 2>&1)
            $exitCode = $LASTEXITCODE
        }
        finally {
            Pop-Location
        }
    }
    finally {
        [Environment]::SetEnvironmentVariable('MODULARPIPELINES_AGENT_LOCK_OWNER_ID', $previousOwnerId, 'Process')
        [Environment]::SetEnvironmentVariable('CODEX_THREAD_ID', $previousThreadId, 'Process')
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

function Assert-Result {
    param(
        [Parameter(Mandatory)]
        $Result,

        [Parameter(Mandatory)]
        [int]$ExpectedExitCode,

        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string]$ExpectedOutput,

        [Parameter(Mandatory)]
        [string]$Operation
    )

    if ($Result.ExitCode -ne $ExpectedExitCode -or $Result.Output -ne $ExpectedOutput) {
        throw "$Operation returned exit $($Result.ExitCode) / '$($Result.Output)', expected exit $ExpectedExitCode / '$ExpectedOutput'"
    }
}

function Invoke-Git {
    param([Parameter(ValueFromRemainingArguments)] [string[]]$Arguments)

    & git @Arguments 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "git command failed: git $($Arguments -join ' ')"
    }
}

function Expire-TestLock([string]$LockName) {
    $deleted = & docker exec $redisContainer redis-cli DEL "modularpipelines:agent-lock:$LockName" "modularpipelines:agent-lock-meta:$LockName"
    if ($LASTEXITCODE -ne 0 -or [int]$deleted -lt 1) {
        throw "failed to expire test lock $LockName"
    }
}

function Assert-OwnerIdentityNotExposed([string]$LockName, [string]$OwnerId) {
    $meta = & docker exec $redisContainer redis-cli GET "modularpipelines:agent-lock-meta:$LockName"
    $ownerHash = [Convert]::ToHexString(
        [System.Security.Cryptography.SHA256]::HashData([System.Text.Encoding]::UTF8.GetBytes($OwnerId)))

    if ($meta -match [regex]::Escape($OwnerId) -or $meta -match [regex]::Escape($ownerHash)) {
        throw "lock metadata exposed owner identity for $LockName"
    }
}

function Remove-TestLocks([string[]]$LockNames) {
    $keys = @($LockNames | ForEach-Object { "modularpipelines:agent-lock:$_"; "modularpipelines:agent-lock-meta:$_" })
    & docker exec $redisContainer redis-cli DEL @keys 2>$null | Out-Null
}

try {
    New-Item -ItemType Directory -Force $repo | Out-Null
    Invoke-Git init --initial-branch=main $repo
    Invoke-Git -C $repo config user.email 'agent-lock-tests@example.invalid'
    Invoke-Git -C $repo config user.name 'Agent Lock Tests'
    Set-Content -LiteralPath (Join-Path $repo 'fixture.txt') -Value 'fixture'
    Invoke-Git -C $repo add fixture.txt
    Invoke-Git -C $repo commit -m 'test fixture'
    Invoke-Git -C $repo worktree add -b "issue-$issueNumber-token-owner-test" $issueWorktree HEAD
    Invoke-Git -C $repo worktree add -b token-owner-pr-test $prWorktree HEAD

    # A checked-out PR may carry a stale lock script. Every verb must invoke the
    # canonical shared-checkout script captured before entering the worktree.
    $staleAgentLocks = Join-Path $prWorktree 'scripts/AgentLocks.ps1'
    New-Item -ItemType Directory -Force (Split-Path $staleAgentLocks -Parent) | Out-Null
    Set-Content -LiteralPath $staleAgentLocks -Value @(
        '[Console]::Error.WriteLine(''stale worktree AgentLocks.ps1 was invoked'')'
        'exit 99'
    )

    # Prove the worktree-local control would fail every verb if a caller regressed
    # from the canonical absolute path to a relative script path.
    $staleControl = Invoke-AgentLocks $prWorktree @('status', '-LockName', $crossVersionLock) `
        -EnvironmentOwnerId $crossVersionOwner -ScriptPath './scripts/AgentLocks.ps1'
    Assert-Result $staleControl 99 'stale worktree AgentLocks.ps1 was invoked' 'stale worktree control'

    Assert-ExitZero (Invoke-AgentLocks $repo @('acquire', '-LockName', $crossVersionLock) -EnvironmentOwnerId $crossVersionOwner) 'cross-version acquire'
    Assert-Result (Invoke-AgentLocks $prWorktree @('status', '-LockName', $crossVersionLock) -EnvironmentOwnerId $crossVersionOwner) 0 'HELD-BY-ME' 'cross-version status from stale worktree'
    Assert-Result (Invoke-AgentLocks $prWorktree @('release', '-LockName', $crossVersionLock) -EnvironmentOwnerId $crossVersionOwner) 0 '' 'cross-version release from stale worktree'
    Assert-Result (Invoke-AgentLocks $repo @('status', '-LockName', $crossVersionLock) -EnvironmentOwnerId $crossVersionOwner) 0 'FREE' 'cross-version status after release'

    # Owner A's token cache must remain private after Redis expiry and owner B reacquisition.
    Assert-ExitZero (Invoke-AgentLocks $repo @('acquire', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerA) 'owner A acquire'
    Assert-Result (Invoke-AgentLocks $repo @('status', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerA) 0 'HELD-BY-ME' 'owner A status before expiry'

    Expire-TestLock $ownershipLock

    Assert-ExitZero (Invoke-AgentLocks $repo @('acquire', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerB) 'owner B acquire'
    Assert-OwnerIdentityNotExposed $ownershipLock $ownerB
    Assert-Result (Invoke-AgentLocks $repo @('status', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerA) 0 'HELD' 'owner A status after reacquire'
    Assert-Result (Invoke-AgentLocks $repo @('renew', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerA) 4 'LOST' 'owner A renew after reacquire'
    Assert-Result (Invoke-AgentLocks $repo @('release', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerA) 5 'STALE' 'owner A release after reacquire'

    Assert-Result (Invoke-AgentLocks $repo @('status', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerB) 0 'HELD-BY-ME' 'owner B status after owner A attempts'
    Assert-Result (Invoke-AgentLocks $repo @('renew', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerB) 0 '' 'owner B renew'
    Assert-Result (Invoke-AgentLocks $repo @('release', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerB) 0 '' 'owner B release'
    Assert-Result (Invoke-AgentLocks $repo @('status', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerB) 0 'FREE' 'owner B status after release'

    # CODEX_THREAD_ID must support short-lived pwsh calls and implicit issue-branch lock names.
    Assert-ExitZero (Invoke-AgentLocks $issueWorktree @('acquire') -CodexThreadId $issueOwner) 'implicit issue acquire'
    Assert-Result (Invoke-AgentLocks $issueWorktree @('status') -CodexThreadId $issueOwner) 0 'HELD-BY-ME' 'implicit issue status'
    Assert-Result (Invoke-AgentLocks $issueWorktree @('renew') -CodexThreadId $issueOwner) 0 '' 'implicit issue renew'
    Assert-Result (Invoke-AgentLocks $issueWorktree @('release') -CodexThreadId $issueOwner) 0 '' 'implicit issue release'

    # Explicit parameter identity must override both environment identities for PR worktree flow.
    $parameterArgs = @('-OwnerId', $parameterOwner)
    Assert-ExitZero (Invoke-AgentLocks $prWorktree (@('acquire', '-LockName', $prWorktreeLock, '-Worktree', $prWorktree) + $parameterArgs) -EnvironmentOwnerId $ownerA -CodexThreadId $ownerB) 'parameter owner PR acquire'
    Assert-Result (Invoke-AgentLocks $prWorktree (@('status') + $parameterArgs) -EnvironmentOwnerId $ownerA -CodexThreadId $ownerB) 0 'HELD-BY-ME' 'parameter owner PR status'
    Assert-Result (Invoke-AgentLocks $prWorktree (@('renew') + $parameterArgs) -EnvironmentOwnerId $ownerA -CodexThreadId $ownerB) 0 '' 'parameter owner PR renew'
    Assert-Result (Invoke-AgentLocks $prWorktree (@('release') + $parameterArgs) -EnvironmentOwnerId $ownerA -CodexThreadId $ownerB) 0 '' 'parameter owner PR release'

    # Non-Codex callers must provide stable identity instead of falling back machine-wide.
    $missingIdentity = Invoke-AgentLocks $repo @('status', '-LockName', $missingIdentityLock)
    if ($missingIdentity.ExitCode -ne 2 -or $missingIdentity.Output -notmatch '^cannot resolve lock owner identity:') {
        throw "missing identity returned exit $($missingIdentity.ExitCode) / '$($missingIdentity.Output)'"
    }

    $completed = $true
    Write-Host 'OK AgentLocks token ownership tests passed.'
}
finally {
    if (-not $completed -and (Test-Path -LiteralPath $repo)) {
        Invoke-AgentLocks $repo @('release', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerB | Out-Null
        Invoke-AgentLocks $repo @('release', '-LockName', $ownershipLock) -EnvironmentOwnerId $ownerA | Out-Null
        Invoke-AgentLocks $repo @('release', '-LockName', $issueLock) -CodexThreadId $issueOwner | Out-Null
        Invoke-AgentLocks $repo @('release', '-LockName', $prWorktreeLock, '-OwnerId', $parameterOwner) | Out-Null
        Invoke-AgentLocks $repo @('release', '-LockName', $crossVersionLock) -EnvironmentOwnerId $crossVersionOwner | Out-Null
    }

    Remove-TestLocks $allLocks

    $tempRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
    $resolvedTestRoot = [System.IO.Path]::GetFullPath($testRoot)
    if (-not $resolvedTestRoot.StartsWith($tempRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove test directory outside temp: $resolvedTestRoot"
    }

    Remove-Item -LiteralPath $resolvedTestRoot -Recurse -Force -ErrorAction SilentlyContinue
}
