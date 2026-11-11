#!/usr/bin/env pwsh
# Local regression suite: requires Git, PowerShell 7 and Docker.
$ErrorActionPreference = 'Stop'
# These fixtures exercise only the explicitly single-machine Docker mode.
$previousRedisMode = $env:MODULARPIPELINES_AGENT_REDIS_MODE
$previousRedisEndpoint = $env:MODULARPIPELINES_AGENT_REDIS_ENDPOINT
$env:MODULARPIPELINES_AGENT_REDIS_MODE = 'local'
$env:MODULARPIPELINES_AGENT_REDIS_ENDPOINT = ''
$agentLocks = Join-Path $PSScriptRoot 'AgentLocks.ps1'
$suffix = [Guid]::NewGuid().ToString('N')
$testRoot = Join-Path ([IO.Path]::GetTempPath()) "released-worktree-tests-$suffix"
$repo = Join-Path $testRoot 'repo'
$owner = "release-tests-$suffix"
$locks = [Collections.Generic.List[string]]::new()

function Invoke-TestGit {
    & git @args 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Git failed: $args" }
}
function Invoke-TestLock([string[]]$Arguments) {
    $start = [Diagnostics.ProcessStartInfo]::new('pwsh')
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.WorkingDirectory = (Get-Location).ProviderPath
    foreach ($argument in @('-NoProfile', '-File', $agentLocks) + $Arguments + @('-OwnerId', $owner)) {
        $start.ArgumentList.Add($argument)
    }
    $process = [Diagnostics.Process]::new()
    $process.StartInfo = $start
    try {
        $null = $process.Start()
        # Drain both streams concurrently so a full stderr pipe cannot block stdout.
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        $process.WaitForExit()
        return [pscustomobject]@{
            Verb = $Arguments[0]
            ExitCode = $process.ExitCode
            StdOut = $stdout.GetAwaiter().GetResult()
            StdErr = $stderr.GetAwaiter().GetResult()
        }
    }
    finally { $process.Dispose() }
}
function Format-TestLockResult($Result) {
    return "AgentLocks $($Result.Verb): exit=$($Result.ExitCode)`nstdout: [$($Result.StdOut)]`nstderr: [$($Result.StdErr)]"
}
function Assert-ReleasedLock($Release, $Status, [string]$Name) {
    $diagnostics = (Format-TestLockResult $Release) + "`n" + (Format-TestLockResult $Status)
    if ($Release.ExitCode -ne 0 -or $Status.ExitCode -ne 0) {
        throw "$Name release/status invocation failed.`n$diagnostics"
    }
    if ($Status.StdOut.TrimEnd("`r", "`n") -cne 'FREE') {
        throw "$Name lock was not released (expected exactly FREE).`n$diagnostics"
    }
}
function Lock {
    $script:lastLockResult = Invoke-TestLock -Arguments $args
    if ($args[0] -in 'release', 'status') { Write-Host (Format-TestLockResult $script:lastLockResult) }
    if ($script:lastLockResult.ExitCode -ne 0) { throw (Format-TestLockResult $script:lastLockResult) }
    if ($args[0] -eq 'release') { $null = $locks.Remove($args[2]) }
}
function Assert([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}
function New-Checkout([string]$Name, [switch]$Detached) {
    $path = Join-Path $testRoot $Name
    if ($Detached) { Invoke-TestGit -C $repo worktree add --detach $path HEAD }
    else { Invoke-TestGit -C $repo worktree add -b $Name $path HEAD }
    return $path
}
function Claim([string]$Name, [string]$Path) {
    $key = "release-test-$suffix-$Name"
    $locks.Add($key)
    Lock acquire -LockName $key -Worktree $Path
    return $key
}

try {
    New-Item -ItemType Directory -Path $repo -Force | Out-Null
    Invoke-TestGit init --initial-branch=main $repo
    Invoke-TestGit -C $repo config user.email 'cleanup-tests@example.invalid'
    Invoke-TestGit -C $repo config user.name 'Cleanup Tests'
    Set-Content (Join-Path $repo '.gitignore') "bin/`n.env`n*.log"
    Set-Content (Join-Path $repo 'source.txt') 'committed'
    Invoke-TestGit -C $repo add .
    Invoke-TestGit -C $repo commit -m fixture
    Push-Location $repo

    # A stale registration must not prevent releasing another valid checkout.
    $missing = New-Checkout missing
    Remove-Item -LiteralPath $missing -Recurse -Force

    $path = New-Checkout clean
    Set-Content (Join-Path $path 'source.txt') 'unpublished commit'
    Invoke-TestGit -C $path commit -am unpublished
    $head = & git -C $path rev-parse HEAD
    New-Item -ItemType Directory (Join-Path $path 'bin') | Out-Null
    Set-Content (Join-Path $path 'bin/generated.dll') 'generated'
    $key = Claim clean $path
    Lock renew -LockName $key # Must retain the path without repeating -Worktree.
    # Release from inside the target must also work on Windows.
    Push-Location $path
    try { Lock release -LockName $key } finally { Pop-Location }
    Assert (-not (Test-Path $path)) 'Clean checkout survived release.'
    Assert ((& git -C $repo rev-parse clean) -eq $head) 'Unpublished branch was lost.'
    Invoke-TestGit -C $repo worktree add $path clean
    Assert ((& git -C $path rev-parse HEAD) -eq $head) 'Cannot re-checkout retained branch.'

    $path = New-Checkout detached -Detached
    Set-Content (Join-Path $path 'source.txt') 'detached commit'
    Invoke-TestGit -C $path commit -am detached
    $head = & git -C $path rev-parse HEAD
    $key = Claim detached $path
    Lock release -LockName $key
    Assert (-not (Test-Path $path)) 'Detached checkout survived release.'
    Assert ((& git -C $repo rev-parse "refs/heads/retained-worktrees/$head") -eq $head) 'Detached commit was lost.'

    if (-not $IsWindows) {
        # A linked ancestor reproduces /var versus /private/var on macOS,
        # including on systems whose temporary directory has no built-in alias.
        $alias = Join-Path $testRoot 'alias'
        New-Item -ItemType SymbolicLink -Path $alias -Target $testRoot | Out-Null
        try {
            foreach ($kind in @('alias-clean', 'alias-dirty', 'alias-subdirectory', 'alias-harness')) {
                $path = if ($kind -eq 'alias-harness') {
                    $nested = Join-Path $repo '.worktrees/alias-harness'
                    Invoke-TestGit -C $repo worktree add -b $kind $nested HEAD
                    $nested
                } else { New-Checkout $kind }
                $aliasedPath = Join-Path $alias ([IO.Path]::GetRelativePath($testRoot, $path))
                if ($kind -eq 'alias-dirty') { Set-Content (Join-Path $path 'source.txt') 'pending change' }
                if ($kind -eq 'alias-subdirectory') {
                    $aliasedPath = Join-Path $aliasedPath 'nested'
                    New-Item -ItemType Directory $aliasedPath | Out-Null
                }
                $key = Claim $kind $aliasedPath
                Lock release -LockName $key
                $release = $script:lastLockResult
                if ($kind -eq 'alias-clean') {
                    Assert (-not (Test-Path $path)) 'Clean aliased checkout survived release.'
                } else {
                    Assert (Test-Path $path) "$kind checkout was removed."
                    $expected = switch ($kind) {
                        alias-dirty { 'Preserving dirty worktree:' }
                        alias-subdirectory { 'Not a worktree root:' }
                        alias-harness { 'Preserving main or harness-managed worktree:' }
                    }
                    Assert (($release.StdOut + $release.StdErr).Contains($expected)) "$kind did not reach the expected safety check: $(Format-TestLockResult $release)"
                }
                $status = Invoke-TestLock -Arguments @('status', '-LockName', $key)
                Assert-ReleasedLock -Release $release -Status $status -Name $kind
            }
        } finally { Remove-Item -LiteralPath $alias -Force }
    }

    if ($IsWindows) {
        # Git for Windows follows a junction while deleting a worktree and empties its
        # target. Release must detach the link so content outside the worktree survives.
        $outside = Join-Path $testRoot 'outside-store'
        New-Item -ItemType Directory $outside | Out-Null
        Set-Content (Join-Path $outside 'keep.txt') 'outside the worktree'
        $path = New-Checkout junction
        New-Item -ItemType Directory (Join-Path $path 'bin') | Out-Null
        New-Item -ItemType Junction -Path (Join-Path $path 'bin/pkg') -Target $outside | Out-Null
        $key = Claim junction $path
        Lock release -LockName $key
        Assert (-not (Test-Path $path)) 'Checkout containing a junction survived release.'
        Assert (Test-Path (Join-Path $outside 'keep.txt')) 'Release deleted content through a junction.'
    }

    if ($IsWindows) {
        # A parent shell started inside the worktree blocks deletion of that directory.
        # Release must leave a pending removal that the sweep finishes after it exits.
        . (Join-Path $PSScriptRoot 'WorktreeCleanup.ps1')
        New-Item -ItemType Directory (Join-Path $repo 'scripts') -Force | Out-Null
        # The sweep runs the shared checkout's lock CLI, which dot-sources its siblings.
        foreach ($script in @('AgentLocks.ps1', 'WorktreeCleanup.ps1', 'Remove-ReleasedWorktree.ps1')) {
            Copy-Item (Join-Path $PSScriptRoot $script) (Join-Path $repo "scripts/$script")
        }
        foreach ($kind in @('busy', 'busy-new-work')) {
            $path = New-Checkout $kind
            New-Item -ItemType Directory (Join-Path $path 'bin') | Out-Null
            Set-Content (Join-Path $path 'bin/generated.dll') 'generated'
            $key = Claim $kind $path
            $holder = Start-Process pwsh -ArgumentList '-NoProfile', '-Command', 'Start-Sleep 300' -WorkingDirectory $path -WindowStyle Hidden -PassThru
            try {
                Start-Sleep -Seconds 2
                Lock release -LockName $key
                Assert (Test-Path $path) "$kind checkout was removed while a process used it; the test cannot reproduce the lock."
                $records = @(Get-ChildItem (Join-Path $repo '.git/agent-pending-removals') -Filter '*.json')
                Assert ($records.Count -eq 1) "$kind release did not record a pending removal."
                if ($kind -eq 'busy-new-work') { Set-Content (Join-Path $path 'notes.md') 'written after release' }
            }
            finally { Stop-Process -Id $holder.Id -Force -ErrorAction SilentlyContinue; $holder.WaitForExit() }
            $null = Complete-PendingWorktreeRemovals -Repo $repo
            Assert (-not (Test-Path (Join-Path $repo '.git/agent-pending-removals/*.json'))) "$kind pending removal record survived the sweep."
            if ($kind -eq 'busy') {
                Assert (-not (Test-Path $path)) 'Pending removal was not completed after the process exited.'
                Assert ((& git -C $repo worktree list --porcelain) -notcontains "worktree $($path -replace '\\', '/')") 'Pending removal left a registration.'
            }
            else {
                Assert (Test-Path (Join-Path $path 'notes.md')) 'Pending removal deleted work written after release.'
            }
        }
    }

    foreach ($kind in @('tracked', 'untracked', 'ignored', 'locked', 'main', 'foreign', 'harness', 'reassigned')) {
        $path = switch ($kind) {
            main { $repo }
            foreign {
                $foreign = Join-Path $testRoot 'foreign'
                Invoke-TestGit init $foreign
                $foreign
            }
            harness {
                $nested = Join-Path $repo '.worktrees/harness'
                Invoke-TestGit -C $repo worktree add -b harness $nested HEAD
                $nested
            }
            default { New-Checkout $kind }
        }
        switch ($kind) {
            tracked { Set-Content (Join-Path $path 'source.txt') 'pending change' }
            untracked { Set-Content (Join-Path $path 'new-source.cs') 'pending source' }
            ignored { Set-Content (Join-Path $path '.env') 'keep settings' }
            locked { Invoke-TestGit -C $repo worktree lock $path }
        }
        $key = Claim $kind $path
        if ($kind -eq 'reassigned') { Invoke-TestGit -C $path config --worktree agent.lockName 'another-item' }
        Lock release -LockName $key
        $release = $script:lastLockResult
        Assert (Test-Path $path) "$kind checkout was removed."
        $status = Invoke-TestLock -Arguments @('status', '-LockName', $key)
        Write-Host (Format-TestLockResult $status)
        Assert-ReleasedLock -Release $release -Status $status -Name $kind
    }
    Write-Host 'OK released worktree cleanup: generated output, unpublished branches, detached commits, re-checkout, busy-directory pending removal, dirty/ignored/locked/main/foreign/harness/reassigned preservation.'
} finally {
    try {
        if (Test-Path $repo) {
            Set-Location $repo
            foreach ($key in $locks.ToArray()) {
                # Do not let a failed status probe prevent release of a lock we acquired.
                try { Lock release -LockName $key }
                catch { Write-Warning "Cleanup release failed for ${key}: $_" }
            }
        }
        Pop-Location -ErrorAction SilentlyContinue
        $resolved = [IO.Path]::GetFullPath($testRoot)
        $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
        if (-not $resolved.StartsWith($temp, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe test cleanup path.' }
        Remove-Item -LiteralPath $resolved -Recurse -Force
    }
    finally {
        $env:MODULARPIPELINES_AGENT_REDIS_MODE = $previousRedisMode
        $env:MODULARPIPELINES_AGENT_REDIS_ENDPOINT = $previousRedisEndpoint
    }
}

