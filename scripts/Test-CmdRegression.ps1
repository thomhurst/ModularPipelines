#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'
$script = Join-Path $PSScriptRoot 'Invoke-CmdRegression.ps1'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) "cmd-regression-$([guid]::NewGuid())"
$null = New-Item -ItemType Directory $testRoot
$guard = Join-Path $testRoot 'Guard.ps1'
@'
param([switch]$SingleNode, [int]$TimeoutSeconds, [string]$LogPath, [string[]]$DotNetArguments)
[pscustomobject]@{
    Timeout = $TimeoutSeconds
    SingleNode = [bool]$SingleNode
    Arguments = $DotNetArguments
} | ConvertTo-Json -Compress | Add-Content (Join-Path $env:CASE_ROOT 'calls.jsonl')
Set-Content -LiteralPath $LogPath -Value "phase $($DotNetArguments[0])"
if ($DotNetArguments[0] -eq $env:FAIL_PHASE) { exit ([int]$env:FAIL_CODE) }
exit 0
'@ | Set-Content -LiteralPath $guard

function Invoke-Case {
    param([string]$Name, [int]$Expected, [int]$Phases, [string]$FailPhase = '', [int]$FailCode = 0,
        [string]$ProbeCommand = 'exit 0', [int]$ProbeTimeout = 10, [switch]$MissingProbe)

    $directory = Join-Path $testRoot $Name
    $null = New-Item -ItemType Directory $directory
    $start = [Diagnostics.ProcessStartInfo]::new((Get-Process -Id $PID).Path)
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.Environment['CASE_ROOT'] = $directory
    $start.Environment['FAIL_PHASE'] = $FailPhase
    $start.Environment['FAIL_CODE'] = $FailCode.ToString()
    # -Command preserves array binding for the probe's arguments.
    $literal = { param($value) "'" + $value.Replace("'", "''") + "'" }
    $probePath = if ($MissingProbe) { Join-Path $directory 'missing.exe' } else { (Get-Process -Id $PID).Path }
    $command = "& $(& $literal $script) -LogDirectory $(& $literal $directory) -GuardScript $(& $literal $guard) " +
        "-ProbePath $(& $literal $probePath) -ProbeArguments @('-NoProfile','-NonInteractive','-Command',$(& $literal $ProbeCommand)) -ProbeTimeoutSeconds $ProbeTimeout; exit `$LASTEXITCODE"
    foreach ($argument in @('-NoProfile', '-NonInteractive', '-Command', $command)) { $start.ArgumentList.Add($argument) }
    $process = [Diagnostics.Process]::Start($start)
    try {
        $output = $process.StandardOutput.ReadToEndAsync()
        $errors = $process.StandardError.ReadToEndAsync()
        if (-not $process.WaitForExit(30000)) {
            $process.Kill($true)
            throw "$Name exceeded its outer 30s deadline."
        }
        $text = $output.GetAwaiter().GetResult() + $errors.GetAwaiter().GetResult()
        Set-Content -LiteralPath (Join-Path $directory 'output.log') -Value $text
        if ($process.ExitCode -ne $Expected) { throw "$Name returned $($process.ExitCode); expected $Expected. $text" }
        $calls = @(Get-Content (Join-Path $directory 'calls.jsonl') | ConvertFrom-Json)
        if ($calls.Count -ne $Phases) { throw "$Name ran $($calls.Count) phases; expected $Phases." }
        if ($text -notmatch 'Windows startup snapshot:') { throw "$Name lost the in-process diagnostics." }
        return $calls
    }
    finally { $process.Dispose() }
}

try {
    . (Join-Path $PSScriptRoot 'WindowsStartupDiagnostics.ps1')
    $record = [pscustomobject]@{ ProviderName = 'Application Error'; Id = 1000; TimeCreated = [datetime]::UtcNow }
    $record | Add-Member ScriptMethod ToXml {
        '<Event><EventData><Data Name="AppName">pwsh.exe</Data><Data Name="ModuleName">user32.dll</Data><Data Name="ExceptionCode">c0000142</Data><Data Name="AppPath">SECRET-PATH</Data><Data Name="CommandLine">SECRET-ARGUMENT</Data><Data Name="Message">SECRET-MESSAGE</Data></EventData></Event>'
    }
    $eventJson = ConvertTo-WindowsStartupEvent -Record $record -LogName Application | ConvertTo-Json -Depth 4
    if ($eventJson -match 'SECRET' -or $eventJson -notmatch 'user32.dll' -or $eventJson -notmatch 'c0000142') {
        throw 'Event allowlist lost diagnostic fields or exposed unfiltered data.'
    }
    $calls = @(Invoke-Case -Name success -Expected 0 -Phases 3)
    if (($calls.Timeout -join ',') -ne '180,300,120') { throw 'Guard phase budgets changed.' }
    if (($calls.SingleNode -join ',') -ne 'False,True,False') { throw 'Single-node build flag changed.' }
    if ('--disable-parallel' -notin $calls[0].Arguments -or '--no-restore' -notin $calls[1].Arguments -or
        '--no-build' -notin $calls[2].Arguments -or '--report-trx' -notin $calls[2].Arguments) {
        throw 'Restore/build/test invocation flags changed.'
    }
    $null = Invoke-Case -Name restore-failure -Expected 7 -Phases 1 -FailPhase restore -FailCode 7
    $null = Invoke-Case -Name memory-limit -Expected 137 -Phases 2 -FailPhase build -FailCode 137
    $null = Invoke-Case -Name test-failure -Expected 42 -Phases 3 -FailPhase run -FailCode 42
    $null = Invoke-Case -Name probe-failure -Expected 23 -Phases 1 -ProbeCommand 'exit 23'
    $null = Invoke-Case -Name preserve-phase-failure -Expected 7 -Phases 1 -FailPhase restore -FailCode 7 -ProbeCommand 'exit 23'
    $null = Invoke-Case -Name missing-probe -Expected 126 -Phases 1 -MissingProbe
    $null = Invoke-Case -Name probe-timeout -Expected 124 -Phases 1 -ProbeCommand 'Start-Sleep -Seconds 20' -ProbeTimeout 1
    if ($IsWindows) {
        $null = Invoke-Case -Name dll-init-failure -Expected -1073741502 -Phases 1 -ProbeCommand 'exit -1073741502'
    }
    Write-Output 'OK: phase budgets and arguments, phase failures, probe failure, missing executable, probe timeout and event sanitization.'
}
finally {
    # Only remove the exact randomly-created test directory under the OS temp root.
    $resolved = [IO.Path]::GetFullPath($testRoot)
    $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
    if (-not $resolved.StartsWith($temp, [StringComparison]::OrdinalIgnoreCase)) { throw 'Test root escaped temp.' }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
