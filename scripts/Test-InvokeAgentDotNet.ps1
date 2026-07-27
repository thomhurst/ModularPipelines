$ErrorActionPreference = 'Stop'

$guardScript = Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("agent-dotnet-guard-{0}" -f [guid]::NewGuid())
$captureScript = Join-Path $testRoot 'Capture-Invocation.ps1'
$timeoutScript = Join-Path $testRoot 'Spawn-Child.ps1'
$capturePath = Join-Path $testRoot 'capture.json'
$childPidPath = Join-Path $testRoot 'child.pid'
$pwshPath = (Get-Process -Id $PID).Path

$resolvedTempRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
$resolvedTestRoot = [System.IO.Path]::GetFullPath($testRoot)
if (-not $resolvedTestRoot.StartsWith($resolvedTempRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to use test directory outside temp root: $resolvedTestRoot"
}

function ConvertTo-PowerShellLiteral([string]$Value) {
    return "'" + $Value.Replace("'", "''") + "'"
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

    @'
param([string]$ChildPidPath)

$child = Start-Process `
    -FilePath (Get-Process -Id $PID).Path `
    -ArgumentList '-NoProfile', '-Command', 'Start-Sleep -Seconds 60' `
    -PassThru
Set-Content -LiteralPath $ChildPidPath -Value $child.Id
Start-Sleep -Seconds 60
'@ | Set-Content -LiteralPath $timeoutScript

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 2 `
        -MemoryLimitMb 512 `
        -PollIntervalMilliseconds 100 `
        -Arguments @('-NoProfile', '-File', $timeoutScript, $childPidPath)

    if ($guardExitCode -ne 124) {
        throw "Timeout returned $guardExitCode instead of 124."
    }

    $childPid = [int](Get-Content -LiteralPath $childPidPath -Raw)
    if (Get-Process -Id $childPid -ErrorAction SilentlyContinue) {
        throw "Timed-out descendant process $childPid is still running."
    }

    $guardExitCode = Invoke-Guard `
        -TimeoutSeconds 30 `
        -MemoryLimitMb 64 `
        -PollIntervalMilliseconds 100 `
        -Arguments @('-NoProfile', '-Command', 'Start-Sleep -Seconds 60')

    if ($guardExitCode -ne 137) {
        throw "Memory limit returned $guardExitCode instead of 137."
    }

    Write-Output 'OK guarded dotnet forwarding, timeout cleanup, and memory limit passed.'
}
finally {
    if (Test-Path -LiteralPath $childPidPath) {
        $childPid = [int](Get-Content -LiteralPath $childPidPath -Raw)
        Stop-Process -Id $childPid -Force -ErrorAction SilentlyContinue
    }

    if (Test-Path -LiteralPath $testRoot) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
