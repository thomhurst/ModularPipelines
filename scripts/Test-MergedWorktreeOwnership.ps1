# Runs the real sweep against a disposable repository and a fake canonical lock CLI.
# No GitHub requests or shared Redis keys are used.
$ErrorActionPreference = 'Stop'
$sweepScript = Join-Path $PSScriptRoot 'Remove-MergedWorktrees.ps1'
$fixtureRoot = Join-Path ([IO.Path]::GetTempPath()) "merged-ownership-$([Guid]::NewGuid().ToString('N'))"
$fixtureRepo = Join-Path $fixtureRoot 'repo'
$fixtureWorktrees = Join-Path $fixtureRoot 'repo-worktrees'
$fixtureState = Join-Path $fixtureRoot 'locks.json'
$fixtureCalls = Join-Path $fixtureRoot 'calls.txt'
$fixtureLockScript = Join-Path $fixtureRepo 'scripts/AgentLocks.ps1'
$fixtureStates = @{}

function Invoke-FixtureGit {
    & git @args 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Fixture Git failed: $args" }
}

function Set-FixtureLock([string]$Name, [string]$State) {
    $script:fixtureStates[$Name] = $State
    $script:fixtureStates | ConvertTo-Json | Set-Content -LiteralPath $fixtureState
}

function New-FixtureWorktree([string]$Name, [string]$Marker = '', [string]$Branch = '') {
    $path = Join-Path $fixtureWorktrees $Name
    if ($Branch) { Invoke-FixtureGit -C $fixtureRepo worktree add -b $Branch $path HEAD }
    else { Invoke-FixtureGit -C $fixtureRepo worktree add --detach $path HEAD }
    if ($Marker) { Invoke-FixtureGit -C $path config --worktree agent.lockName $Marker }
    return $path
}

function Assert-Fixture([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

function Invoke-FixtureSweep([switch]$Preview) {
    $output = @(& $sweepScript -Repo fixture/repo -WhatIf:$Preview 6>&1)
    Assert-Fixture (($output -join "`n") -notmatch 'TOKEN-SHOULD-NOT-LEAK') 'Cleanup printed a lock token.'
}

function gh {
    $global:LASTEXITCODE = 0
    if ($args[0] -eq 'pr' -and $args[1] -eq 'list') {
        if ($args -contains 'merged') {
            return ConvertTo-Json -Compress -InputObject @(@{
                number = 999999; mergedAt = '2026-01-01T00:00:00Z'
                headRefName = 'merged-fixture'; headRefOid = $script:fixtureHead
            })
        }
        return '[]'
    }
    if ($args[0] -eq 'api') { return '[]' }
    throw "Unexpected GitHub invocation in fixture: $args"
}

try {
    New-Item -ItemType Directory -Path (Split-Path $fixtureLockScript), $fixtureWorktrees -Force | Out-Null
    @'
param([string]$Verb, [string]$LockName, [string]$OwnerId)
$statePath = Join-Path $PSScriptRoot '../../locks.json'
$callsPath = Join-Path $PSScriptRoot '../../calls.txt'
$states = Get-Content -LiteralPath $statePath -Raw | ConvertFrom-Json -AsHashtable
$state = $states[$LockName]
Add-Content -LiteralPath $callsPath -Value "$Verb $LockName $OwnerId"
if ($state -eq 'ERROR') { exit 1 }
switch ($Verb) {
    status {
        if ($state -eq 'HELD' -or $state -like 'OWNED:*') { 'HELD' }
        else { 'FREE' }
    }
    acquire {
        if ($state -eq 'HELD' -or $state -eq 'RACE' -or $state -like 'OWNED:*') { exit 3 }
        $states[$LockName] = "OWNED:$OwnerId"
        $states | ConvertTo-Json | Set-Content -LiteralPath $statePath
        'TOKEN-SHOULD-NOT-LEAK'
    }
    release {
        if ($state -ne "OWNED:$OwnerId") { exit 5 }
        $states[$LockName] = 'FREE'
        $states | ConvertTo-Json | Set-Content -LiteralPath $statePath
    }
    default { exit 1 }
}
exit 0
'@ | Set-Content -LiteralPath $fixtureLockScript
    '{}' | Set-Content -LiteralPath $fixtureState
    Invoke-FixtureGit init --initial-branch=main $fixtureRepo
    Invoke-FixtureGit -C $fixtureRepo config user.name 'Cleanup fixture'
    Invoke-FixtureGit -C $fixtureRepo config user.email 'cleanup@example.invalid'
    Invoke-FixtureGit -C $fixtureRepo config extensions.worktreeConfig true
    Invoke-FixtureGit -C $fixtureRepo add .
    Invoke-FixtureGit -C $fixtureRepo commit -m fixture
    Invoke-FixtureGit -C $fixtureRepo remote add origin $fixtureRepo
    $script:fixtureHead = & git -C $fixtureRepo rev-parse HEAD
    Push-Location $fixtureRepo
    try {
        $path = New-FixtureWorktree 'pr-900001-setup'
        Set-FixtureLock 'pr-900001' 'HELD'
        Invoke-FixtureSweep
        Assert-Fixture (Test-Path -LiteralPath $path) 'Active detached setup checkout was removed before its marker existed.'
        Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path

        foreach ($kind in @('dangling', 'markerless')) {
            foreach ($state in @('HELD', 'ERROR', 'FREE')) {
                $path = Join-Path $fixtureWorktrees "pr-999999-$kind-$state"
                New-Item -ItemType Directory -Path $path | Out-Null
                $source = Join-Path $path 'unpublished.txt'
                Set-Content -LiteralPath $source -Value 'source from before the merge'
                (Get-Item -LiteralPath $source).LastWriteTimeUtc = [datetime]'2025-01-01T00:00:00Z'
                if ($kind -eq 'dangling') {
                    Set-Content -LiteralPath (Join-Path $path '.git') -Value "gitdir: $fixtureRepo/.git/worktrees/missing"
                }
                Set-FixtureLock 'pr-999999' $state
                Invoke-FixtureSweep -Preview
                Assert-Fixture (Test-Path -LiteralPath $source) "Preview deleted a $kind orphan."
                Invoke-FixtureSweep
                if ($state -eq 'FREE') {
                    Assert-Fixture (-not (Test-Path -LiteralPath $path)) "Released $kind orphan was not removed."
                }
                else {
                    Assert-Fixture (Test-Path -LiteralPath $source) "$kind orphan was deleted while ownership was $state."
                    $resolvedPath = [IO.Path]::GetFullPath($path)
                    $orphanRoot = [IO.Path]::GetFullPath($fixtureWorktrees) + [IO.Path]::DirectorySeparatorChar
                    if (-not $resolvedPath.StartsWith($orphanRoot, [StringComparison]::OrdinalIgnoreCase)) {
                        throw 'Refusing cleanup outside the fixture worktree root.'
                    }
                    Remove-Item -LiteralPath $resolvedPath -Recurse -Force
                }
            }
        }

        $unidentifiedOrphan = Join-Path $fixtureWorktrees 'unknown-owner'
        New-Item -ItemType Directory -Path $unidentifiedOrphan | Out-Null
        Set-Content -LiteralPath (Join-Path $unidentifiedOrphan '.git') -Value "gitdir: $fixtureRepo/.git/worktrees/missing"
        Invoke-FixtureSweep
        Assert-Fixture (Test-Path -LiteralPath $unidentifiedOrphan) 'Orphan without recoverable ownership was deleted.'

        $path = New-FixtureWorktree 'renamed-review' -Marker 'pr-900002' -Branch 'renamed-review'
        Set-FixtureLock 'pr-900002' 'HELD'
        Invoke-FixtureSweep
        Assert-Fixture (Test-Path -LiteralPath $path) 'Active marked worktree was removed after its branch was renamed.'
        Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path

        $path = New-FixtureWorktree 'branch-only' -Branch 'review/pr-900005-check'
        Set-FixtureLock 'pr-900005' 'HELD'
        Invoke-FixtureSweep
        Assert-Fixture (Test-Path -LiteralPath $path) 'Active branch identity was ignored.'
        Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path

        $path = New-FixtureWorktree 'pr-900006-dirty'
        Set-FixtureLock 'pr-900006' 'FREE'
        Set-Content -LiteralPath (Join-Path $path 'unpublished.txt') -Value 'valuable source'
        Invoke-FixtureSweep
        Assert-Fixture (Test-Path -LiteralPath (Join-Path $path 'unpublished.txt')) 'Free ownership bypassed source preservation.'
        $states = Get-Content -LiteralPath $fixtureState -Raw | ConvertFrom-Json -AsHashtable
        Assert-Fixture ($states['pr-900006'] -eq 'FREE') 'Preserving dirty work leaked a reservation.'
        Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path

        $path = New-FixtureWorktree 'pr-900003-released' -Marker 'pr-900003'
        Set-FixtureLock 'pr-900003' 'FREE'
        Invoke-FixtureSweep
        Assert-Fixture (-not (Test-Path -LiteralPath $path)) 'Released merged worktree was not removed.'
        $calls = Get-Content -LiteralPath $fixtureCalls
        Assert-Fixture ([bool]($calls -match '^acquire pr-900003 ')) 'Cleanup did not reserve its item lock before deleting.'
        Assert-Fixture ([bool]($calls -match '^release pr-900003 ')) 'Cleanup did not release its reservation.'

        foreach ($state in @('ERROR', 'RACE')) {
            $path = New-FixtureWorktree "pr-900004-$state"
            Set-FixtureLock 'pr-900004' $state
            Invoke-FixtureSweep
            Assert-Fixture (Test-Path -LiteralPath $path) "Worktree was removed when ownership returned $state."
            Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path
        }

        $path = New-FixtureWorktree 'pr-900011-multiple' -Marker 'issue-900010'
        Set-FixtureLock 'issue-900010' 'FREE'
        Set-FixtureLock 'pr-900011' 'HELD'
        Invoke-FixtureSweep
        Assert-Fixture (Test-Path -LiteralPath $path) 'A second active identity was ignored.'
        $states = Get-Content -LiteralPath $fixtureState -Raw | ConvertFrom-Json -AsHashtable
        Assert-Fixture ($states['issue-900010'] -eq 'FREE') 'Partial cleanup reservation leaked.'
        Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path

        $path = New-FixtureWorktree 'pr-900012-preview'
        Set-FixtureLock 'pr-900012' 'FREE'
        Clear-Content -LiteralPath $fixtureCalls
        Invoke-FixtureSweep -Preview
        Assert-Fixture (Test-Path -LiteralPath $path) 'WhatIf removed a worktree.'
        $calls = Get-Content -LiteralPath $fixtureCalls
        Assert-Fixture (-not [bool]($calls -match '^(acquire|release) ')) 'WhatIf mutated lock ownership.'
        Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path

        $path = New-FixtureWorktree 'pr-900013-unavailable'
        Set-FixtureLock 'pr-900013' 'FREE'
        Move-Item -LiteralPath $fixtureLockScript -Destination "$fixtureLockScript.disabled"
        Invoke-FixtureSweep
        Assert-Fixture (Test-Path -LiteralPath $path) 'Missing canonical lock script did not preserve the worktree.'
        Move-Item -LiteralPath "$fixtureLockScript.disabled" -Destination $fixtureLockScript
        Invoke-FixtureGit -C $fixtureRepo worktree remove --force $path
    }
    finally { Pop-Location }
}
finally {
    $resolvedRoot = [IO.Path]::GetFullPath($fixtureRoot)
    $tempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([char[]]@('/', '\')) + [IO.Path]::DirectorySeparatorChar
    if (-not $resolvedRoot.StartsWith($tempRoot, [StringComparison]::OrdinalIgnoreCase) -or
        (Split-Path $resolvedRoot -Leaf) -notlike 'merged-ownership-*') {
        throw 'Refusing cleanup outside the disposable fixture root.'
    }
    Remove-Item -LiteralPath $resolvedRoot -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Output 'OK merged-worktree ownership guards passed.'
