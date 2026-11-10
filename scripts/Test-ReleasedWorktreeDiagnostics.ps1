#!/usr/bin/env pwsh
# Exercise the cleanup suite's actual subprocess capture and assertions without Docker.
$ErrorActionPreference = 'Stop'
$parseErrors = $null
$ast = [Management.Automation.Language.Parser]::ParseFile(
    (Join-Path $PSScriptRoot 'Test-ReleasedWorktreeCleanup.ps1'), [ref]$null, [ref]$parseErrors)
if ($parseErrors.Count) { throw ($parseErrors -join "`n") }
foreach ($name in 'Invoke-TestLock', 'Format-TestLockResult', 'Assert-ReleasedLock') {
    $definition = $ast.Find({ param($node)
        $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $name
    }, $false)
    if (-not $definition) { throw "Missing function $name" }
    . ([scriptblock]::Create($definition.Extent.Text))
}

$testRoot = Join-Path ([IO.Path]::GetTempPath()) "released-worktree-diagnostics-$([Guid]::NewGuid().ToString('N'))"
$agentLocks = Join-Path $testRoot 'AgentLocks.ps1'
$owner = 'diagnostics-fixture'
function Assert-Failure($Release, $Status, [string[]]$Expected) {
    $failure = $null
    try { Assert-ReleasedLock -Release $Release -Status $Status -Name locked }
    catch { $failure = $_.Exception.Message }
    if (-not $failure) { throw 'Accepted a failed invocation or a non-FREE status.' }
    foreach ($text in $Expected) {
        if (-not $failure.Contains($text)) { throw "Missing diagnostic '$text': $failure" }
    }
}

try {
    New-Item -ItemType Directory -Path $testRoot | Out-Null
    @'
param([string]$Verb, [string]$LockName, [string]$OwnerId)
if ($Verb -eq 'release') {
    Write-Output 'Preserving main or Git-locked worktree: fixture'
    [Console]::Error.WriteLine('release diagnostic')
    exit 0
}
[Console]::Error.WriteLine('Docker daemon unavailable')
exit 1
'@ | Set-Content -LiteralPath $agentLocks
    $release = Invoke-TestLock -Arguments @('release', '-LockName', 'fixture')
    $status = Invoke-TestLock -Arguments @('status', '-LockName', 'fixture')
    if ($release.ExitCode -ne 0 -or $release.StdOut.Trim() -cne 'Preserving main or Git-locked worktree: fixture' -or
        $release.StdErr.Trim() -cne 'release diagnostic' -or $status.ExitCode -ne 1 -or
        $status.StdOut -cne '' -or $status.StdErr.Trim() -cne 'Docker daemon unavailable') {
        throw 'Subprocess capture lost an exit code or mixed stdout and stderr.'
    }
    Assert-Failure $release $status @('locked release/status invocation failed.', 'release: exit=0',
        'Preserving main or Git-locked worktree: fixture', 'release diagnostic', 'status: exit=1', 'Docker daemon unavailable')

    # The old comparison accepts FREE even with a failed invocation or extra lines.
    $status.ExitCode = 17
    $status.StdOut = "FREE`n"
    Assert-Failure $release $status @('invocation failed.', 'status: exit=17', 'stdout: [FREE')
    $status.ExitCode = 0
    $status.StdErr = ''
    $status.StdOut = "unexpected output`nFREE`n"
    Assert-Failure $release $status @('expected exactly FREE', 'unexpected output', 'status: exit=0')
    $status.StdOut = "HELD-BY-ME`n"
    Assert-Failure $release $status @('locked lock was not released', 'HELD-BY-ME', 'release diagnostic')
    $status.StdOut = "FREE`r`n"
    Assert-ReleasedLock -Release $release -Status $status -Name locked
    $release.ExitCode = 5
    Assert-Failure $release $status @('invocation failed.', 'release: exit=5', 'status: exit=0')
    Write-Output 'OK released worktree diagnostics: separate streams, exit codes, failed probes, retained locks, strict FREE status.'
}
finally {
    $resolved = [IO.Path]::GetFullPath($testRoot)
    $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
    if (-not $resolved.StartsWith($temp, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe test cleanup path.' }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
