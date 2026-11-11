#!/usr/bin/env pwsh
# Real Git fixtures for released-worktree path validation; no Redis required.
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'WorktreeCleanup.ps1')
. (Join-Path $PSScriptRoot 'Remove-ReleasedWorktree.ps1')

$testRoot = Join-Path ([IO.Path]::GetTempPath()) "released-worktree-paths-$([Guid]::NewGuid().ToString('N'))"
$physicalRoot = Join-Path $testRoot 'physical'
$aliasRoot = Join-Path $testRoot 'alias'
$repo = Join-Path $physicalRoot 'repo'
$lockName = 'issue-path-fixture'
$previousDirectory = [Environment]::CurrentDirectory

function Invoke-FixtureGit {
    & git @args 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Fixture Git failed: $args" }
}
function Assert([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}
function New-DirectoryAlias([string]$Path, [string]$Target) {
    $kind = if ($IsWindows) { 'Junction' } else { 'SymbolicLink' }
    New-Item -ItemType $kind -Path $Path -Target $Target | Out-Null
}
function New-FixtureWorktree([string]$Name) {
    $path = Join-Path $physicalRoot $Name
    $alias = Join-Path $aliasRoot $Name
    Invoke-FixtureGit -C $repo worktree add --detach $path HEAD
    Invoke-FixtureGit -C $path config --worktree agent.lockName $lockName
    Invoke-FixtureGit -C $path config --worktree agent.lockWorktree $alias
    return $path
}
function Invoke-FixtureRelease([string]$Path) {
    return (@(Remove-ReleasedWorktree -Worktree $Path -LockName $lockName 6>&1) -join "`n")
}
function Assert-Rejected([string]$Path, [string]$Expected) {
    $failure = $null
    try { $null = Invoke-FixtureRelease $Path }
    catch { $failure = $_.Exception.Message }
    Assert ($failure -like "$Expected*") "Expected '$Expected', got '$failure'."
    Assert (Test-Path -LiteralPath $Path) "Rejected path was removed: $Path"
}

try {
    New-Item -ItemType Directory -Path $repo -Force | Out-Null
    New-DirectoryAlias $aliasRoot $physicalRoot
    Invoke-FixtureGit init --initial-branch=main $repo
    Invoke-FixtureGit -C $repo config user.email 'path-tests@example.invalid'
    Invoke-FixtureGit -C $repo config user.name 'Worktree Path Tests'
    Invoke-FixtureGit -C $repo config extensions.worktreeConfig true
    Set-Content -LiteralPath (Join-Path $repo 'source.txt') 'committed'
    Invoke-FixtureGit -C $repo add .
    Invoke-FixtureGit -C $repo commit -m fixture
    Push-Location (Join-Path $aliasRoot 'repo')

    # Both the release path and ownership marker use an alias in an ancestor.
    $path = New-FixtureWorktree dirty
    Set-Content -LiteralPath (Join-Path $path 'source.txt') 'uncommitted'
    $output = Invoke-FixtureRelease (Join-Path $aliasRoot 'dirty')
    Assert ($output -like 'Preserving dirty worktree:*') "Alias did not reach dirty preservation: $output"
    Assert ((Get-Content -LiteralPath (Join-Path $path 'source.txt')) -eq 'uncommitted') 'Dirty source was lost.'
    $output = Invoke-FixtureRelease (Join-Path '..' 'dirty')
    Assert ($output -like 'Preserving dirty worktree:*') "Relative alias did not reach dirty preservation: $output"

    $path = New-FixtureWorktree clean
    $output = Invoke-FixtureRelease (Join-Path $aliasRoot 'clean')
    Assert ($output -like '*Removed released worktree:*') "Clean alias was not removed: $output"
    Assert (-not (Test-Path -LiteralPath $path)) 'Clean checkout survived release.'
    Assert (Test-Path -LiteralPath $physicalRoot) 'Release removed the alias target parent.'

    # A leaf alias must identify the same checkout too.
    $path = New-FixtureWorktree leaf
    $leafAlias = Join-Path $testRoot 'leaf-alias'
    New-DirectoryAlias $leafAlias (Join-Path $aliasRoot 'leaf')
    Invoke-FixtureGit -C $path config --worktree agent.lockWorktree $leafAlias
    try {
        $output = Invoke-FixtureRelease $leafAlias
        Assert ($output -like '*Removed released worktree:*') "Leaf alias was not removed: $output"
        Assert (-not (Test-Path -LiteralPath $path)) 'Leaf alias left the physical checkout.'
    }
    finally { Remove-Item -LiteralPath $leafAlias -Force -ErrorAction SilentlyContinue }

    # Normalizing aliases must not authorize subdirectories, another repository,
    # the main checkout, nested harness checkouts, or another checkout's marker.
    $path = New-FixtureWorktree sibling
    Invoke-FixtureGit -C $path config --worktree agent.lockWorktree (Join-Path $aliasRoot 'dirty')
    $output = Invoke-FixtureRelease (Join-Path $aliasRoot 'sibling')
    Assert ($output -like 'Preserving worktree without a matching lock marker:*') "Copied marker was trusted: $output"
    $subdirectory = Join-Path $path 'subdirectory'
    New-Item -ItemType Directory -Path $subdirectory | Out-Null
    Assert-Rejected $subdirectory 'Not a worktree root:'

    $foreign = Join-Path $physicalRoot 'foreign'
    Invoke-FixtureGit init $foreign
    Assert-Rejected (Join-Path $aliasRoot 'foreign') 'Worktree belongs to another repository:'

    $output = Invoke-FixtureRelease (Join-Path $aliasRoot 'repo')
    Assert ($output -like 'Preserving main or Git-locked worktree:*') "Main alias was not protected: $output"
    $harness = Join-Path $repo '.worktrees/harness'
    Invoke-FixtureGit -C $repo worktree add --detach $harness HEAD
    Invoke-FixtureGit -C $harness config --worktree agent.lockName $lockName
    Invoke-FixtureGit -C $harness config --worktree agent.lockWorktree $harness
    $harnessAlias = Join-Path $physicalRoot 'harness-alias'
    New-DirectoryAlias $harnessAlias $harness
    $output = Invoke-FixtureRelease $harnessAlias
    Assert ($output -like 'Preserving main or harness-managed worktree:*') "Nested checkout escaped through alias: $output"
    Assert (Test-Path -LiteralPath $harness) 'Harness checkout was removed.'

    Write-Output 'OK released worktree paths: ancestor/leaf aliases, dirty preservation, clean removal, root/repository/ownership/harness guards.'
}
finally {
    Pop-Location -ErrorAction SilentlyContinue
    [Environment]::CurrentDirectory = $previousDirectory
    # Detach every directory link before recursively removing the verified fixture.
    foreach ($link in @($leafAlias, $harnessAlias, $aliasRoot)) {
        if ($link) { Remove-Item -LiteralPath $link -Force -ErrorAction SilentlyContinue }
    }
    $resolved = [IO.Path]::GetFullPath($testRoot)
    $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
    if (-not $resolved.StartsWith($temp, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe test cleanup path.' }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
