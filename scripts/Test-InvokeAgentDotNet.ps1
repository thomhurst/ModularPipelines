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
$orphanReadyPath = Join-Path $testRoot 'orphan-ready.pid'
$boundedChildPidPath = Join-Path $testRoot 'bounded-child.pid'
$waitsPath = Join-Path $testRoot 'waits.txt'
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

function Get-TestProcessStartIdentity {
    param([int]$ProcessId)

    if ($IsLinux) {
        # .NET derives StartTime from a per-process boot-time estimate on Linux.
        # Use the kernel start token so separate PowerShell hosts agree on identity.
        $stat = [System.IO.File]::ReadAllText("/proc/$ProcessId/stat")
        $fields = $stat.Substring($stat.LastIndexOf(')') + 2).Split(
            ' ', [System.StringSplitOptions]::RemoveEmptyEntries)
        return $fields[19]
    }

    $candidate = [System.Diagnostics.Process]::GetProcessById($ProcessId)
    try {
        return $candidate.StartTime.ToUniversalTime().Ticks.ToString(
            [Globalization.CultureInfo]::InvariantCulture)
    }
    finally {
        $candidate.Dispose()
    }
}

function Test-SavedProcessIsRunning($Identity) {
    try {
        $candidate = [System.Diagnostics.Process]::GetProcessById($Identity.ProcessId)
        try {
            return (-not $candidate.HasExited) -and
                (Get-TestProcessStartIdentity $Identity.ProcessId) -eq $Identity.StartIdentity
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
            if ((Get-TestProcessStartIdentity $Identity.ProcessId) -eq $Identity.StartIdentity) {
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
    [string[]]$Arguments,
    [string]$DotNetPath = $pwshPath,
    [switch]$SingleNode,
    [string]$Location,
    [string]$ReadyPath,
    [string]$WaitsPath) {
    $invocationScript = $guardScript
    if ($ReadyPath) {
        $invocationScript = Write-ReadyGuard -ReadyPath $ReadyPath -WaitsPath $WaitsPath
    }

    $argumentLiterals = ($Arguments | ForEach-Object { ConvertTo-PowerShellLiteral $_ }) -join ', '
    $command = ''
    if ($Location) {
        # Only the PowerShell location changes; the process working directory stays
        # where the test runner started, which is exactly the agent-worktree scenario.
        $command += "Set-Location -LiteralPath $(ConvertTo-PowerShellLiteral $Location); "
    }

    $command += "& $(ConvertTo-PowerShellLiteral $invocationScript) " +
        "-DotNetPath $(ConvertTo-PowerShellLiteral $DotNetPath) " +
        "-TimeoutSeconds $TimeoutSeconds " +
        "-MemoryLimitMb $MemoryLimitMb " +
        "-PollIntervalMilliseconds $PollIntervalMilliseconds "
    if ($SingleNode) {
        $command += '-SingleNode '
    }

    $command += "-DotNetArguments @($argumentLiterals)"
    $command += '; exit $LASTEXITCODE'
    $encodedCommand = [Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($command))

    & $pwshPath -NoProfile -OutputFormat Text -EncodedCommand $encodedCommand
    return $LASTEXITCODE
}

function Write-ReadyGuard([string]$ReadyPath, [string]$WaitsPath) {
    # Instrument a temporary copy, leaving the production deadline and API unchanged.
    # Start the short timeout at fixture readiness, and observe the actual wait argument
    # rather than timing PowerShell startup, native compilation, snapshots, or cleanup.
    $tokens = $null
    $parseErrors = $null
    $ast = [System.Management.Automation.Language.Parser]::ParseFile(
        $guardScript, [ref]$tokens, [ref]$parseErrors)
    if ($parseErrors) {
        throw "Could not parse guard: $parseErrors"
    }

    $remainingAssignments = @($ast.FindAll({
        param($node)
        $node -is [System.Management.Automation.Language.AssignmentStatementAst] -and
            $node.Left.Extent.Text -eq '$remaining'
    }, $true))
    $waitCalls = @($ast.FindAll({
        param($node)
        $node -is [System.Management.Automation.Language.InvokeMemberExpressionAst] -and
            $node.Expression.Extent.Text -eq '$process' -and
            $node.Member.Value -eq 'WaitForExit' -and $node.Arguments.Count -eq 1
    }, $true))
    if ($remainingAssignments.Count -ne 1 -or $waitCalls.Count -ne 1) {
        throw 'Guard monitoring changed; update the readiness and wait observation boundaries.'
    }

    $readyLiteral = ConvertTo-PowerShellLiteral $ReadyPath
    $waitsLiteral = ConvertTo-PowerShellLiteral $WaitsPath
    $readiness = @'
if (-not $testFixtureReady) {
    $testReadyDeadline = [DateTimeOffset]::UtcNow.AddSeconds(30)
    while (-not (Test-Path -LiteralPath {{READY}})) {
        if ($process.HasExited -or [DateTimeOffset]::UtcNow -ge $testReadyDeadline) {
            throw 'Descendant fixture did not report readiness within 30 seconds.'
        }
        Start-Sleep -Milliseconds 50
    }

    $testIdentity = Get-Content -LiteralPath {{READY}} -Raw | ConvertFrom-Json
    $testChild = [Diagnostics.Process]::GetProcessById($testIdentity.ProcessId)
    try {
        if ($testChild.HasExited -or
            (Get-TestProcessStartIdentity $testIdentity.ProcessId) -ne $testIdentity.StartIdentity) {
            throw "Descendant fixture $($testIdentity.ProcessId) was not alive at readiness."
        }
    }
    finally {
        $testChild.Dispose()
    }

    $deadline = [DateTimeOffset]::UtcNow.AddSeconds($TimeoutSeconds)
    $testFixtureReady = $true
    Set-Content -LiteralPath ({{READY}} + '.go') -Value 'ready'
}

'@
    $source = $ast.Extent.Text
    $waitArgument = $waitCalls[0].Arguments[0].Extent
    # This wrapper returns the original value and still calls the real WaitForExit.
    # Observing its argument also catches regressions that bypass $waitMilliseconds.
    $observation = '(& { param($value) Add-Content -LiteralPath ' + $waitsLiteral +
        ' -Value $value; return $value } (' + $waitArgument.Text + '))'
    $edits = @(
        @{ Start = $ast.ParamBlock.Extent.EndOffset; Length = 0;
            Text = "`n$identityFunction`n" },
        @{ Start = $remainingAssignments[0].Extent.StartOffset; Length = 0;
            Text = $readiness.Replace('{{READY}}', $readyLiteral) },
        @{ Start = $waitArgument.StartOffset; Length = $waitArgument.EndOffset - $waitArgument.StartOffset;
            Text = $observation }
    )
    foreach ($edit in ($edits | Sort-Object Start -Descending)) {
        $source = $source.Remove($edit.Start, $edit.Length).Insert($edit.Start, $edit.Text)
    }

    $instrumentedPath = Join-Path $testRoot 'Ready-Guard.ps1'
    Set-Content -LiteralPath $instrumentedPath -Value $source
    return $instrumentedPath
}

function Write-DotNetShim([string]$Directory, [string]$CapturePath) {
    # A native shim stands in for dotnet so the recorded working directory and arguments
    # reflect exactly what the guard launched, without a second PowerShell parameter
    # binder in the way. It writes its working directory, then one argument per line.
    if ($IsWindows) {
        $shimPath = Join-Path $Directory 'dotnet-shim.cmd'
        $shimScript = @'
@echo off
> "{{CAPTURE}}" echo %CD%
:loop
if "%~1"=="" exit /b 0
>> "{{CAPTURE}}" echo %~1
shift
goto loop
'@
    }
    else {
        $shimPath = Join-Path $Directory 'dotnet-shim.sh'
        $shimScript = @'
#!/bin/sh
pwd > '{{CAPTURE}}'
for a in "$@"; do printf '%s\n' "$a" >> '{{CAPTURE}}'; done
'@
    }

    [System.IO.File]::WriteAllText(
        $shimPath,
        ($shimScript.Replace('{{CAPTURE}}', $CapturePath) -replace "`r`n", "`n") + "`n",
        [System.Text.UTF8Encoding]::new($false))
    if (-not $IsWindows) {
        & chmod +x $shimPath
    }

    return $shimPath
}

function Assert-ShimCapture(
    [string]$CapturePath,
    [string]$ExpectedWorkingDirectory,
    [string[]]$ExpectedArguments,
    [string]$Scenario) {
    $lines = @(Get-Content -LiteralPath $CapturePath)
    $actualWorkingDirectory = [System.IO.Path]::GetFullPath($lines[0])
    $expectedWorkingDirectory = [System.IO.Path]::GetFullPath($ExpectedWorkingDirectory)
    if ($actualWorkingDirectory -ne $expectedWorkingDirectory) {
        throw "$Scenario ran dotnet in '$actualWorkingDirectory' instead of '$expectedWorkingDirectory'."
    }

    $actualArguments = @($lines | Select-Object -Skip 1)
    if (($actualArguments -join '|') -ne ($ExpectedArguments -join '|')) {
        throw "$Scenario forwarded '$($actualArguments -join '|')' instead of '$($ExpectedArguments -join '|')'."
    }
}

$identityFunction = 'function Get-TestProcessStartIdentity {' +
    ${function:Get-TestProcessStartIdentity}.ToString() + "`n}"
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

    # Agents run the guard after Set-Location into an isolated worktree. That changes only
    # the PowerShell location, so the launched dotnet must be pinned to it explicitly, and
    # MSBuild-style '-name:value' tokens must reach dotnet unsplit.
    $shimDirectory = Join-Path $testRoot 'shim'
    $probeDirectory = Join-Path $testRoot 'worktree-probe'
    New-Item -ItemType Directory -Path $shimDirectory, $probeDirectory | Out-Null
    $shimCapturePath = Join-Path $testRoot 'shim-capture.txt'
    $shimPath = Write-DotNetShim -Directory $shimDirectory -CapturePath $shimCapturePath

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -DotNetPath $shimPath `
        -Location $probeDirectory `
        -SingleNode `
        -Arguments @('build', 'Project.slnx', '-c', 'Release', '-v:q', 'two words', '--', '-t:Build')

    if ($guardExitCode -ne 0) {
        throw "Single-node worktree build command failed with exit code $guardExitCode."
    }

    Assert-ShimCapture `
        -CapturePath $shimCapturePath `
        -ExpectedWorkingDirectory $probeDirectory `
        -ExpectedArguments @('build', 'Project.slnx', '-c', 'Release', '-v:q', 'two words', '-m:1', '--', '-t:Build') `
        -Scenario 'Single-node worktree build'

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -DotNetPath $shimPath `
        -SingleNode `
        -Arguments @('build', 'Project.slnx', '/m:2', '-v:q')

    if ($guardExitCode -ne 0) {
        throw "Explicit node-count build command failed with exit code $guardExitCode."
    }

    Assert-ShimCapture `
        -CapturePath $shimCapturePath `
        -ExpectedWorkingDirectory ([System.Environment]::CurrentDirectory) `
        -ExpectedArguments @('build', 'Project.slnx', '/m:2', '-v:q') `
        -Scenario 'Explicit node-count build'

    (@'
param(
    [string]$ChildPidPath,
    [int]$ExitAfterSpawn = 0,
    [int]$StartupDelayMilliseconds = 0
)

{{IDENTITY_FUNCTION}}

Start-Sleep -Milliseconds $StartupDelayMilliseconds
# Descendants must survive until guard cleanup; a natural exit could hide a leak.
$startProcessParameters = @{
    FilePath = (Get-Process -Id $PID).Path
    ArgumentList = @('-NoProfile', '-Command', 'while ($true) { Start-Sleep -Seconds 60 }')
    PassThru = $true
}
if ($IsWindows) {
    $startProcessParameters.WindowStyle = 'Hidden'
}

$child = Start-Process @startProcessParameters
[pscustomobject]@{
    ProcessId = $child.Id
    StartIdentity = Get-TestProcessStartIdentity $child.Id
} | ConvertTo-Json | Set-Content -LiteralPath "$ChildPidPath.tmp"
Move-Item -LiteralPath "$ChildPidPath.tmp" -Destination $ChildPidPath
while (-not (Test-Path -LiteralPath "$ChildPidPath.go")) {
    Start-Sleep -Milliseconds 50
}
if (-not $ExitAfterSpawn) {
    while ($true) { Start-Sleep -Seconds 60 }
}
'@).Replace('{{IDENTITY_FUNCTION}}', $identityFunction) | Set-Content -LiteralPath $timeoutScript

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -ReadyPath $normalExitChildPidPath `
        -WaitsPath $waitsPath `
        -Arguments @('-NoProfile', '-File', $timeoutScript, $normalExitChildPidPath, '1')

    if ($guardExitCode -ne 0) {
        throw "Normal-exit cleanup command failed with exit code $guardExitCode."
    }

    $normalExitChild = Get-SavedProcessIdentity $normalExitChildPidPath
    if (Test-SavedProcessIsRunning $normalExitChild) {
        throw "Normal-exit descendant process $($normalExitChild.ProcessId) is still running."
    }

    # Deliberately take longer to become ready than the two-second guard budget.
    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 2 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -ReadyPath $childPidPath `
        -WaitsPath $waitsPath `
        -Arguments @('-NoProfile', '-File', $timeoutScript, $childPidPath, '0', '3000')

    if ($guardExitCode -ne 124) {
        throw "Timeout returned $guardExitCode instead of 124."
    }

    $timedOutChild = Get-SavedProcessIdentity $childPidPath
    if (Test-SavedProcessIsRunning $timedOutChild) {
        throw "Timed-out descendant process $($timedOutChild.ProcessId) is still running."
    }

    if (-not $IsWindows) {
        (@'
param([string]$PidPath)

{{IDENTITY_FUNCTION}}

[pscustomobject]@{
    ProcessId = $PID
    StartIdentity = Get-TestProcessStartIdentity $PID
} | ConvertTo-Json | Set-Content -LiteralPath "$PidPath.tmp"
Move-Item -LiteralPath "$PidPath.tmp" -Destination $PidPath
while ($true) { Start-Sleep -Seconds 60 }
'@).Replace('{{IDENTITY_FUNCTION}}', $identityFunction) | Set-Content -LiteralPath $orphanGrandchildScript

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
    [string]$PidPath,
    [string]$ReadyPath
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
# Publish readiness only after the intermediate exited, proving reparenting happened.
Copy-Item -LiteralPath $PidPath -Destination "$ReadyPath.tmp"
Move-Item -LiteralPath "$ReadyPath.tmp" -Destination $ReadyPath
while ($true) { Start-Sleep -Seconds 60 }
'@ | Set-Content -LiteralPath $orphanParentScript

        $guardExitCode = Invoke-Guard `
            -TimeoutSeconds 4 `
            -MemoryLimitMb 512 `
            -PollIntervalMilliseconds 10000 `
            -ReadyPath $orphanReadyPath `
            -WaitsPath $waitsPath `
            -Arguments @(
                '-NoProfile',
                '-File',
                $orphanParentScript,
                $orphanIntermediateScript,
                $orphanGrandchildScript,
                $orphanPidPath,
                $orphanReadyPath)

        if ($guardExitCode -ne 124) {
            throw "Unix orphan timeout returned $guardExitCode instead of 124."
        }

        $orphan = Get-SavedProcessIdentity $orphanPidPath
        if (Test-SavedProcessIsRunning $orphan) {
            throw "Reparented Unix descendant process $($orphan.ProcessId) is still running."
        }
    }

    Remove-Item -LiteralPath $waitsPath -Force
    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 1 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 10000 `
        -ReadyPath $boundedChildPidPath `
        -WaitsPath $waitsPath `
        -Arguments @('-NoProfile', '-File', $timeoutScript, $boundedChildPidPath)

    if ($guardExitCode -ne 124) {
        throw "Bounded timeout returned $guardExitCode instead of 124."
    }

    $waits = @(Get-Content -LiteralPath $waitsPath | ForEach-Object { [int]$_ })
    if ($waits.Count -eq 0 -or @($waits | Where-Object { $_ -lt 1 -or $_ -gt 1000 }).Count -gt 0) {
        throw "One-second timeout requested an unbounded wait: $($waits -join ', ') milliseconds."
    }
    Write-Output "One-second timeout requested waits of $($waits -join ', ') milliseconds."

    $boundedChild = Get-SavedProcessIdentity $boundedChildPidPath
    if (Test-SavedProcessIsRunning $boundedChild) {
        throw "Bounded-timeout descendant process $($boundedChild.ProcessId) is still running."
    }

    # Retain an uninstrumented timeout check with the production startup deadline.
    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 1 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 10000 `
        -Arguments @('-NoProfile', '-Command', 'Start-Sleep -Seconds 60')
    if ($guardExitCode -ne 124) {
        throw "Production timeout returned $guardExitCode instead of 124."
    }

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 64 `
        -PollIntervalMilliseconds 100 `
        -Arguments @('-NoProfile', '-Command', 'Start-Sleep -Seconds 60')

    if ($guardExitCode -ne 137) {
        throw "Memory limit returned $guardExitCode instead of 137."
    }

    Write-Output 'OK forwarding, worktree working directory, single-node arguments, normal-exit cleanup, bounded timeout, timeout cleanup, and memory limit passed.'
}
finally {
    foreach ($pidPath in @($childPidPath, $normalExitChildPidPath, $orphanPidPath, $boundedChildPidPath)) {
        if (Test-Path -LiteralPath $pidPath) {
            Stop-SavedProcess (Get-SavedProcessIdentity $pidPath)
        }
    }

    if (Test-Path -LiteralPath $testRoot) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
