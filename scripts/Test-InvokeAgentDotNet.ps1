$ErrorActionPreference = 'Stop'

$guardScript = Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("agent-dotnet-guard-{0}" -f [guid]::NewGuid())
$captureScript = Join-Path $testRoot 'Capture-Invocation.ps1'
$timeoutScript = Join-Path $testRoot 'Spawn-Child.ps1'
$orphanParentScript = Join-Path $testRoot 'Spawn-OrphanParent.ps1'
$orphanIntermediateScript = Join-Path $testRoot 'Spawn-OrphanIntermediate.ps1'
$orphanGrandchildScript = Join-Path $testRoot 'OrphanGrandchild.ps1'
$capturePath = Join-Path $testRoot 'capture.json'
$childPidPath = Join-Path $testRoot 'child.pid'
$normalExitChildPidPath = Join-Path $testRoot 'normal-exit-child.pid'
$orphanPidPath = Join-Path $testRoot 'orphan.pid'
$pwshPath = (Get-Process -Id $PID).Path

$resolvedTempRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
$resolvedTestRoot = [System.IO.Path]::GetFullPath($testRoot)
if (-not $resolvedTestRoot.StartsWith($resolvedTempRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to use test directory outside temp root: $resolvedTestRoot"
}

function ConvertTo-PowerShellLiteral([string]$Value) {
    return "'" + $Value.Replace("'", "''") + "'"
}

function Get-SavedProcessIdentity([string]$Path) {
    return Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json
}

function Test-SavedProcessIsRunning($Identity) {
    try {
        $candidate = [System.Diagnostics.Process]::GetProcessById($Identity.ProcessId)
        try {
            return (-not $candidate.HasExited) -and
                $candidate.StartTime.ToUniversalTime().Ticks -eq $Identity.StartTimeUtcTicks
        }
        finally {
            $candidate.Dispose()
        }
    }
    catch [System.ArgumentException] {
        return $false
    }
}

function Stop-SavedProcess($Identity) {
    try {
        $candidate = [System.Diagnostics.Process]::GetProcessById($Identity.ProcessId)
        try {
            if ($candidate.StartTime.ToUniversalTime().Ticks -eq $Identity.StartTimeUtcTicks) {
                $candidate.Kill($true)
            }
        }
        finally {
            $candidate.Dispose()
        }
    }
    catch [System.ArgumentException] {
        Write-Verbose "Saved process $($Identity.ProcessId) already exited."
    }
}

function Invoke-Guard(
    [int]$TimeoutSeconds,
    [int]$MemoryLimitMb,
    [int]$PollIntervalMilliseconds,
    [string[]]$Arguments) {
    $argumentLiterals = ($Arguments | ForEach-Object { ConvertTo-PowerShellLiteral $_ }) -join ', '
    $command = "& $(ConvertTo-PowerShellLiteral $guardScript) " +
        "-DotNetPath $(ConvertTo-PowerShellLiteral $pwshPath) " +
        "-TimeoutSeconds $TimeoutSeconds " +
        "-MemoryLimitMb $MemoryLimitMb " +
        "-PollIntervalMilliseconds $PollIntervalMilliseconds " +
        "-DotNetArguments @($argumentLiterals)"
    $command += '; exit $LASTEXITCODE'
    $encodedCommand = [Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($command))

    & $pwshPath -NoProfile -OutputFormat Text -EncodedCommand $encodedCommand
    return $LASTEXITCODE
}

New-Item -ItemType Directory -Path $testRoot | Out-Null

try {
    @'
param(
    [string]$OutputPath,
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$ForwardedArguments
)

[pscustomobject]@{
    Arguments = $ForwardedArguments
    BuildInParallel = $env:BuildInParallel
    MsBuildDisableNodeReuse = $env:MSBUILDDISABLENODEREUSE
    UseSharedCompilation = $env:UseSharedCompilation
    Priority = if ($IsWindows) {
        (Get-Process -Id $PID).PriorityClass.ToString()
    }
    else {
        (& ps -o ni= -p $PID).Trim()
    }
} | ConvertTo-Json | Set-Content -LiteralPath $OutputPath
'@ | Set-Content -LiteralPath $captureScript

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -Arguments @('-NoProfile', '-File', $captureScript, $capturePath, 'alpha', 'two words')

    if ($guardExitCode -ne 0) {
        throw "Guarded argument-forwarding command failed with exit code $guardExitCode."
    }

    $capture = Get-Content -LiteralPath $capturePath -Raw | ConvertFrom-Json
    if (($capture.Arguments -join '|') -ne 'alpha|two words') {
        throw "Arguments were not forwarded exactly: $($capture.Arguments -join '|')"
    }

    if (($capture.BuildInParallel -ne 'false') -or
        ($capture.MsBuildDisableNodeReuse -ne '1') -or
        ($capture.UseSharedCompilation -ne 'false')) {
        throw 'Guarded build environment was not applied to the child process.'
    }

    if (($IsWindows -and $capture.Priority -ne 'BelowNormal') -or
        ((-not $IsWindows) -and [int]$capture.Priority -le 0)) {
        throw "Guarded child priority was not lowered: $($capture.Priority)"
    }

    @'
param(
    [string]$ChildPidPath,
    [int]$ParentDelayMilliseconds = 60000
)

$startProcessParameters = @{
    FilePath = (Get-Process -Id $PID).Path
    ArgumentList = @('-NoProfile', '-Command', 'Start-Sleep -Seconds 60')
    PassThru = $true
}
if ($IsWindows) {
    $startProcessParameters.WindowStyle = 'Hidden'
}

$child = Start-Process @startProcessParameters
[pscustomobject]@{
    ProcessId = $child.Id
    StartTimeUtcTicks = $child.StartTime.ToUniversalTime().Ticks
} | ConvertTo-Json | Set-Content -LiteralPath $ChildPidPath
Start-Sleep -Milliseconds $ParentDelayMilliseconds
'@ | Set-Content -LiteralPath $timeoutScript

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -Arguments @('-NoProfile', '-File', $timeoutScript, $normalExitChildPidPath, '0')

    if ($guardExitCode -ne 0) {
        throw "Normal-exit cleanup command failed with exit code $guardExitCode."
    }

    $normalExitChild = Get-SavedProcessIdentity $normalExitChildPidPath
    if (Test-SavedProcessIsRunning $normalExitChild) {
        throw "Normal-exit descendant process $($normalExitChild.ProcessId) is still running."
    }

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 2 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -Arguments @('-NoProfile', '-File', $timeoutScript, $childPidPath)

    if ($guardExitCode -ne 124) {
        throw "Timeout returned $guardExitCode instead of 124."
    }

    $timedOutChild = Get-SavedProcessIdentity $childPidPath
    if (Test-SavedProcessIsRunning $timedOutChild) {
        throw "Timed-out descendant process $($timedOutChild.ProcessId) is still running."
    }

    if (-not $IsWindows) {
        @'
param([string]$PidPath)

[pscustomobject]@{
    ProcessId = $PID
    StartTimeUtcTicks = (Get-Process -Id $PID).StartTime.ToUniversalTime().Ticks
} | ConvertTo-Json | Set-Content -LiteralPath $PidPath
Start-Sleep -Seconds 60
'@ | Set-Content -LiteralPath $orphanGrandchildScript

        @'
param(
    [string]$GrandchildScript,
    [string]$PidPath
)

$child = Start-Process `
    -FilePath (Get-Process -Id $PID).Path `
    -ArgumentList @('-NoProfile', '-File', $GrandchildScript, $PidPath) `
    -PassThru

$deadline = [DateTimeOffset]::UtcNow.AddSeconds(10)
while ((-not (Test-Path -LiteralPath $PidPath)) -and
    [DateTimeOffset]::UtcNow -lt $deadline) {
    Start-Sleep -Milliseconds 50
}
'@ | Set-Content -LiteralPath $orphanIntermediateScript

        @'
param(
    [string]$IntermediateScript,
    [string]$GrandchildScript,
    [string]$PidPath
)

$intermediate = Start-Process `
    -FilePath (Get-Process -Id $PID).Path `
    -ArgumentList @(
        '-NoProfile',
        '-File',
        $IntermediateScript,
        $GrandchildScript,
        $PidPath) `
    -PassThru
$intermediate.WaitForExit()
Start-Sleep -Seconds 60
'@ | Set-Content -LiteralPath $orphanParentScript

        $guardExitCode = Invoke-Guard `
            -TimeoutSeconds 4 `
            -MemoryLimitMb 512 `
            -PollIntervalMilliseconds 10000 `
            -Arguments @(
                '-NoProfile',
                '-File',
                $orphanParentScript,
                $orphanIntermediateScript,
                $orphanGrandchildScript,
                $orphanPidPath)

        if ($guardExitCode -ne 124) {
            throw "Unix orphan timeout returned $guardExitCode instead of 124."
        }

        $orphan = Get-SavedProcessIdentity $orphanPidPath
        if (Test-SavedProcessIsRunning $orphan) {
            throw "Reparented Unix descendant process $($orphan.ProcessId) is still running."
        }
    }

    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 1 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 10000 `
        -Arguments @('-NoProfile', '-Command', 'Start-Sleep -Seconds 2')
    $stopwatch.Stop()

    if ($guardExitCode -ne 124) {
        throw "Bounded timeout returned $guardExitCode instead of 124."
    }

    if ($stopwatch.Elapsed -ge [TimeSpan]::FromSeconds(3)) {
        throw "Bounded timeout took too long: $($stopwatch.Elapsed)."
    }

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 64 `
        -PollIntervalMilliseconds 100 `
        -Arguments @('-NoProfile', '-Command', 'Start-Sleep -Seconds 60')

    if ($guardExitCode -ne 137) {
        throw "Memory limit returned $guardExitCode instead of 137."
    }

    Write-Output 'OK forwarding, normal-exit cleanup, bounded timeout, timeout cleanup, and memory limit passed.'
}
finally {
    foreach ($pidPath in @($childPidPath, $normalExitChildPidPath, $orphanPidPath)) {
        if (Test-Path -LiteralPath $pidPath) {
            Stop-SavedProcess (Get-SavedProcessIdentity $pidPath)
        }
    }

    if (Test-Path -LiteralPath $testRoot) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
