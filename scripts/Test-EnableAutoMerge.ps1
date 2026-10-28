# Exercise Enable-AutoMerge.ps1 with the gate and GitHub CLI stubbed out.
$ErrorActionPreference = 'Stop'

$scriptPath = Join-Path $PSScriptRoot 'Enable-AutoMerge.ps1'
$testDirectory = Join-Path ([IO.Path]::GetTempPath()) "modularpipelines-automerge-test-$([guid]::NewGuid().ToString('N'))"
$null = New-Item -ItemType Directory -Path $testDirectory
try {
    $harnessPath = Join-Path $testDirectory 'harness.ps1'
    $gateLogPath = Join-Path $testDirectory 'gate-arguments.json'
    $mergeLogPath = Join-Path $testDirectory 'merge-arguments.json'
    @'
param([string]$ArmScript, [string]$GateLogPath, [string]$MergeLogPath, [string]$GateOutput, [int]$GateExit, [int]$MergeExit)
$ErrorActionPreference = 'Stop'
function pwsh {
    ConvertTo-Json -InputObject @($args) -Compress | Set-Content -LiteralPath $GateLogPath
    $global:LASTEXITCODE = $GateExit
    $GateOutput
}
function gh {
    if ($args[0] -ne 'pr' -or $args[1] -ne 'merge') { throw 'Unexpected GitHub operation.' }
    ConvertTo-Json -InputObject @($args) -Compress | Set-Content -LiteralPath $MergeLogPath
    $global:LASTEXITCODE = $MergeExit
}
& $ArmScript -Pr 123
exit $LASTEXITCODE
'@ | Set-Content -LiteralPath $harnessPath

    function Invoke-Arm([string]$GateOutput, [int]$GateExit = 0, [int]$MergeExit = 0) {
        Remove-Item -LiteralPath $gateLogPath, $mergeLogPath -ErrorAction SilentlyContinue
        $output = & pwsh -NoProfile -File $harnessPath -ArmScript $scriptPath -GateLogPath $gateLogPath `
            -MergeLogPath $mergeLogPath -GateOutput $GateOutput -GateExit $GateExit -MergeExit $MergeExit 2>&1
        [pscustomobject]@{ Exit = $LASTEXITCODE; Output = ($output -join "`n") }
    }

    $validatedSha = 'a' * 40
    $gateResult = @{ headRefOid = $validatedSha; headRefName = 'test-branch' } | ConvertTo-Json -Compress
    $result = Invoke-Arm $gateResult
    if ($result.Exit -ne 0 -or $result.Output -notmatch "Armed auto-merge for #123 at $validatedSha") {
        throw "A passing gate must arm auto-merge. Output: $($result.Output)"
    }
    $gateArguments = @(Get-Content -LiteralPath $gateLogPath -Raw | ConvertFrom-Json)
    foreach ($required in '-ForAutoMerge', '-Json') {
        if ($gateArguments -notcontains $required) { throw "The arming gate must run with $required." }
    }
    $mergeArguments = @(Get-Content -LiteralPath $mergeLogPath -Raw | ConvertFrom-Json)
    foreach ($required in '--auto', '--squash') {
        if ($mergeArguments -notcontains $required) { throw "Auto-merge must be armed with $required." }
    }
    $shaIndex = [Array]::IndexOf($mergeArguments, '--match-head-commit')
    if ($shaIndex -lt 0 -or $mergeArguments[$shaIndex + 1] -ne $validatedSha) {
        throw 'Auto-merge must be pinned to exactly the SHA the gate validated.'
    }

    $result = Invoke-Arm '' 1
    if ($result.Exit -ne 1 -or (Test-Path -LiteralPath $mergeLogPath)) {
        throw 'A gate denial must stop before arming.'
    }
    $result = Invoke-Arm '{}'
    if ($result.Exit -ne 1 -or (Test-Path -LiteralPath $mergeLogPath)) {
        throw 'A gate result without a validated SHA must stop before arming.'
    }
    $result = Invoke-Arm $gateResult 0 1
    if ($result.Exit -ne 1 -or $result.Output -notmatch 'NOT ARMED') {
        throw 'A rejected arming request (for example a moved head) must report failure.'
    }
}
finally {
    $resolvedTestDirectory = [IO.Path]::GetFullPath($testDirectory)
    $expectedParent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([IO.Path]::DirectorySeparatorChar)
    if ([IO.Path]::GetDirectoryName($resolvedTestDirectory) -ne $expectedParent -or
        [IO.Path]::GetFileName($resolvedTestDirectory) -notlike 'modularpipelines-automerge-test-*') {
        throw 'Refusing cleanup outside the temporary auto-merge test directory.'
    }
    Remove-Item -LiteralPath $resolvedTestDirectory -Recurse -Force
}

Write-Output 'OK Enable-AutoMerge gate, validated-head binding, and failure semantics passed.'
