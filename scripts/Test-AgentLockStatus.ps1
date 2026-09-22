# Exercise the real status verb with a fake Docker command; no shared Redis state is touched.
$ErrorActionPreference = 'Stop'
$agentLocks = Join-Path $PSScriptRoot 'AgentLocks.ps1'
$fixtureRoot = Join-Path ([IO.Path]::GetTempPath()) "agent-lock-status-$([Guid]::NewGuid().ToString('N'))"
$runner = Join-Path $fixtureRoot 'runner.ps1'
$calls = Join-Path $fixtureRoot 'calls.txt'

try {
    New-Item -ItemType Directory -Path $fixtureRoot | Out-Null
    @'
param([string]$AgentLocks, [string]$Scenario, [string]$Calls)
function docker {
    Add-Content -LiteralPath $Calls -Value ($args -join ' ')
    $global:LASTEXITCODE = 0
    switch ($args[0]) {
        inspect {
            if ($Scenario -eq 'Missing') { $global:LASTEXITCODE = 1; return }
            if ($Scenario -eq 'Stopped') { return 'false' }
            return 'true'
        }
        { $_ -in 'run', 'start' } { return 'fake-container' }
        exec {
            if ($args -contains 'PING') { return 'PONG' }
            if ($Scenario -in 'Missing', 'Stopped', 'Unavailable') {
                $global:LASTEXITCODE = 1
                return
            }
            if ($Scenario -eq 'Held') { return 'another-owner-token' }
            return ''
        }
        default { throw "Unexpected Docker operation: $args" }
    }
}
& $AgentLocks status -LockName fixture-status -OwnerId fixture-status-reader
exit $LASTEXITCODE
'@ | Set-Content -LiteralPath $runner

    foreach ($scenario in @('Missing', 'Stopped', 'Unavailable', 'Free', 'Held')) {
        Set-Content -LiteralPath $calls -Value ''
        $output = @(& pwsh -NoProfile -File $runner -AgentLocks $agentLocks -Scenario $scenario -Calls $calls 2>&1)
        $exitCode = $LASTEXITCODE
        $operations = Get-Content -LiteralPath $calls
        if ($operations -match '^(run|start|pull) ') {
            throw "Status initialized infrastructure for $scenario."
        }

        if ($scenario -in 'Free', 'Held') {
            if ($exitCode -ne 0 -or ($output -join "`n").Trim() -cne $scenario.ToUpperInvariant()) {
                throw "Status returned an incorrect ownership state for $scenario."
            }
        }
        elseif ($exitCode -ne 1 -or $output -contains 'FREE') {
            throw "Status did not fail closed for $scenario (exit $exitCode)."
        }
    }
}
finally {
    $resolvedRoot = [IO.Path]::GetFullPath($fixtureRoot)
    $tempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([char[]]@('/', '\')) + [IO.Path]::DirectorySeparatorChar
    if (-not $resolvedRoot.StartsWith($tempRoot, [StringComparison]::OrdinalIgnoreCase) -or
        (Split-Path $resolvedRoot -Leaf) -notlike 'agent-lock-status-*') {
        throw 'Refusing cleanup outside the disposable fixture root.'
    }
    Remove-Item -LiteralPath $resolvedRoot -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Output 'OK read-only AgentLocks status passed.'
