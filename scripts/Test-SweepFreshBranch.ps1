# Regression test for Test-HeadHasNoWorkBeyondMain: Remove-MergedWorktrees.ps1 must not
# treat a branch cut from main, with no commits of its own, as merged.
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'WorktreeCleanup.ps1')
$testRoot = Join-Path ([IO.Path]::GetTempPath()) "sweep-fresh-branch-$([Guid]::NewGuid().ToString('N'))"
$repo = Join-Path $testRoot 'repo'

function Invoke-TestGit {
    & git @args 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Git failed: $args" }
}
function Assert([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

try {
    Invoke-TestGit init --initial-branch=main $repo
    Invoke-TestGit -C $repo config user.email 'sweep-tests@example.invalid'
    Invoke-TestGit -C $repo config user.name 'Sweep Tests'
    Invoke-TestGit -C $repo commit --allow-empty -m 'first'
    $behind = (git -C $repo rev-parse HEAD).Trim()
    Invoke-TestGit -C $repo commit --allow-empty -m 'second'
    $mainTip = (git -C $repo rev-parse HEAD).Trim()

    Assert (Test-HeadHasNoWorkBeyondMain -Repo $repo -Head $mainTip -MainTip $mainTip) 'A branch cut at the main tip counted as having its own work.'
    Assert (Test-HeadHasNoWorkBeyondMain -Repo $repo -Head $behind -MainTip $mainTip) 'A branch behind main counted as having its own work.'

    Invoke-TestGit -C $repo switch --quiet -c feature
    Invoke-TestGit -C $repo commit --allow-empty -m 'feature work'
    $featureTip = (git -C $repo rev-parse HEAD).Trim()
    Assert (-not (Test-HeadHasNoWorkBeyondMain -Repo $repo -Head $featureTip -MainTip $mainTip)) 'A branch with its own commit counted as a fresh cut.'

    Assert (Test-HeadHasNoWorkBeyondMain -Repo $repo -Head $featureTip -MainTip $null) 'An unknown main tip did not fail closed.'

    Write-Output 'OK sweep fresh-branch guard: main-tip and behind-main branches kept, branches with work still eligible.'
}
finally {
    $resolved = [IO.Path]::GetFullPath($testRoot)
    $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
    if (-not $resolved.StartsWith($temp, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe test cleanup path.' }
    Remove-Item -LiteralPath $resolved -Recurse -Force -ErrorAction SilentlyContinue
}
