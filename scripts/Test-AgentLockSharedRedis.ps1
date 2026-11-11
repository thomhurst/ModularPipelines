#!/usr/bin/env pwsh
# Shared-backend integration tests. Each worker has an independent token cache.
$ErrorActionPreference = 'Stop'
$agentLocks = Join-Path $PSScriptRoot 'AgentLocks.ps1'
$suffix = [Guid]::NewGuid().ToString('N')
$container = "agent-lock-shared-test-$suffix"
$fixtureRoot = Join-Path ([IO.Path]::GetTempPath()) $container
$lockName = "shared-test-$suffix"
$redisPassword = [Guid]::NewGuid().ToString('N')
$password = $redisPassword
$endpoint = ''

function Invoke-Worker([string]$Worker, [string[]]$Arguments, [string]$EndpointOverride = $endpoint,
    [string]$Mode = 'shared', [string]$Username = '') {
    $cache = Join-Path $fixtureRoot $Worker
    New-Item -ItemType Directory -Force $cache | Out-Null
    $runner = Join-Path $fixtureRoot 'worker.ps1'
    @'
param([string]$AgentLocks, [string]$ArgumentsPath)
$ErrorActionPreference = 'Stop'
[Console]::OutputEncoding = [Text.UTF8Encoding]::new($false)
function docker { throw 'Shared workers must never consult a machine-local Docker daemon.' }
$arguments = @(Get-Content -LiteralPath $ArgumentsPath -Raw | ConvertFrom-Json)
$parameters = @{ Verb = $arguments[0] }
for ($index = 1; $index -lt $arguments.Count; $index += 2) {
    $parameters[$arguments[$index].TrimStart('-')] = $arguments[$index + 1]
}
& $AgentLocks @parameters
exit $LASTEXITCODE
'@ | Set-Content -LiteralPath $runner
    $argumentsPath = Join-Path $cache 'arguments.json'
    ConvertTo-Json -InputObject ($Arguments + @('-LockName', $lockName, '-OwnerId', 'independent-worker')) |
        Set-Content -LiteralPath $argumentsPath
    $start = [Diagnostics.ProcessStartInfo]::new('pwsh')
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.Environment['MODULARPIPELINES_AGENT_REDIS_MODE'] = $Mode
    $start.Environment['MODULARPIPELINES_AGENT_REDIS_ENDPOINT'] = $EndpointOverride
    $start.Environment['MODULARPIPELINES_AGENT_REDIS_PASSWORD'] = $password
    $start.Environment['MODULARPIPELINES_AGENT_REDIS_USERNAME'] = $Username
    foreach ($name in 'TEMP', 'TMP', 'TMPDIR') { $start.Environment[$name] = $cache }
    # Workers deliberately share an identity but not a filesystem or token cache.
    foreach ($argument in @('-NoProfile', '-File', $runner, '-AgentLocks', $agentLocks, '-ArgumentsPath', $argumentsPath)) {
        $start.ArgumentList.Add($argument)
    }
    $process = [Diagnostics.Process]::Start($start)
    try {
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        if (-not $process.WaitForExit(20000)) { $process.Kill($true); throw 'Worker exceeded 20 seconds.' }
        [pscustomobject]@{ ExitCode = $process.ExitCode; Output = $stdout.GetAwaiter().GetResult().Trim(); Error = $stderr.GetAwaiter().GetResult().Trim() }
    } finally { $process.Dispose() }
}
function Assert-Result($Result, [int]$ExitCode, [string]$Output, [string]$Operation) {
    if ($Result.ExitCode -ne $ExitCode -or $Result.Output -cne $Output) {
        throw "$Operation returned exit $($Result.ExitCode), output '$($Result.Output)', error '$($Result.Error)'."
    }
    if ($Result.Error.Contains($password)) { throw "$Operation exposed credentials." }
}

try {
    $missing = Invoke-Worker a @('acquire') ''
    if ($missing.ExitCode -ne 2 -or $missing.Output) { throw "Missing shared configuration did not fail closed: $($missing.ExitCode) $($missing.Error)" }
    $missingDefault = Invoke-Worker a @('acquire') '' -Mode ''
    if ($missingDefault.ExitCode -ne 2 -or $missingDefault.Output) { throw 'Default mode did not require shared configuration.' }
    foreach ($invalid in 'https://example.invalid:6379', 'redis://user:secret@example.invalid', 'redis://example.invalid/1', 'redis://example.invalid?db=0') {
        $result = Invoke-Worker a @('status') $invalid
        if ($result.ExitCode -ne 2 -or $result.Output) { throw 'Invalid endpoint did not fail closed.' }
    }
    $conflicting = Invoke-Worker a @('acquire') 'redis://example.invalid' -Mode local
    if ($conflicting.ExitCode -ne 2 -or $conflicting.Output) { throw 'Conflicting local/shared configuration was accepted.' }
    & docker run -d --name $container -p '127.0.0.1::6379' redis:7-alpine redis-server --requirepass $password | Out-Null
    if ($LASTEXITCODE) { throw 'Could not start disposable Redis.' }
    $port = (& docker port $container 6379/tcp) -replace '^.*:', ''
    $endpoint = "redis://127.0.0.1:$port"
    # Server readiness only; this never waits for a work item.
    for ($attempt = 0; $attempt -lt 30; $attempt++) {
        $ready = & docker exec -e "REDISCLI_AUTH=$password" $container redis-cli PING 2>$null
        if ($LASTEXITCODE -eq 0 -and $ready -eq 'PONG') { break }
        Start-Sleep -Milliseconds 100
    }
    if ($ready -ne 'PONG') { throw 'Disposable Redis did not become ready.' }
    & docker exec -e "REDISCLI_AUTH=$password" $container redis-cli ACL SETUSER fixture-worker on ">$password" '~*' '+@all' | Out-Null
    if ($LASTEXITCODE) { throw 'Could not configure fixture ACL user.' }
    Assert-Result (Invoke-Worker a @('ping') -Username fixture-worker) 0 '' 'Redis ACL authentication'
    $tlsFailure = Invoke-Worker a @('ping') ($endpoint -replace '^redis:', 'rediss:')
    if ($tlsFailure.ExitCode -ne 1 -or $tlsFailure.Output) { throw 'TLS failure did not fail closed.' }

    $acquired = Invoke-Worker a @('acquire')
    if ($acquired.ExitCode -ne 0 -or $acquired.Output -notmatch '^[0-9a-f]{32}$') { throw 'Worker A did not acquire.' }
    $held = Invoke-Worker b @('acquire')
    Assert-Result $held 3 '' 'Competing independent worker'
    if ($held.Error -cne 'HELD') { throw 'Contender did not report HELD.' }
    Assert-Result (Invoke-Worker a @('status')) 0 'HELD-BY-ME' 'Owner status'
    Assert-Result (Invoke-Worker b @('status')) 0 'HELD' 'Independent cache status'
    Assert-Result (Invoke-Worker a @('renew')) 0 '' 'Owner renew'
    Assert-Result (Invoke-Worker a @('defer', '-Fingerprint', "unicode-λ`r`nline", '-DeferSeconds', '60')) 0 '' 'Shared deferral'
    Assert-Result (Invoke-Worker b @('deferred')) 0 "$lockName=unicode-λ`r`nline" 'Shared Unicode bulk/array response'

    # Simulate expiry and acquisition by another machine, using only test keys.
    & docker exec -e "REDISCLI_AUTH=$password" $container redis-cli DEL "modularpipelines:agent-lock:$lockName" "modularpipelines:agent-lock-meta:$lockName" | Out-Null
    if ($LASTEXITCODE) { throw 'Could not expire test lock.' }
    $replacement = Invoke-Worker b @('acquire')
    if ($replacement.ExitCode -ne 0) { throw 'Worker B did not acquire expired lock.' }
    Assert-Result (Invoke-Worker a @('status')) 0 'HELD' 'Lost owner status'
    $lost = Invoke-Worker a @('renew')
    Assert-Result $lost 4 '' 'Lost owner renewal'
    if ($lost.Error -cne 'LOST') { throw 'Lost owner renewal did not report LOST.' }
    $stale = Invoke-Worker a @('release')
    Assert-Result $stale 5 '' 'Stale owner release'
    if ($stale.Error -cne 'STALE') { throw 'Stale release did not report STALE.' }
    Assert-Result (Invoke-Worker b @('status')) 0 'HELD-BY-ME' 'Replacement survives stale release'
    Assert-Result (Invoke-Worker b @('release')) 0 '' 'Replacement release'
    Assert-Result (Invoke-Worker a @('status')) 0 'FREE' 'Released shared lock'

    $acquired = Invoke-Worker a @('acquire')
    if ($acquired.ExitCode -ne 0) { throw 'Could not reacquire before outage checks.' }
    # A different backend must not inherit cached authority to renew a free key.
    $isolated = Invoke-Worker a @('renew') ($endpoint -replace '127.0.0.1', 'localhost')
    if ($isolated.ExitCode -ne 1 -or $isolated.Error -notlike '*no cached token*') { throw 'Backend token caches were not isolated.' }

    # Redis protocol errors often have a zero redis-cli exit code; they must fail closed too.
    $password = 'wrong-password'
    foreach ($verb in 'acquire', 'status', 'renew', 'release', 'ping', 'defer', 'deferred') {
        $arguments = @($verb)
        if ($verb -eq 'defer') { $arguments += @('-Fingerprint', 'unauthenticated') }
        $result = Invoke-Worker a $arguments
        if ($result.ExitCode -ne 1 -or $result.Output) { throw "Authentication failure did not fail closed for $verb." }
    }
    $password = $redisPassword
    & docker stop $container | Out-Null
    if ($LASTEXITCODE) { throw 'Could not stop disposable Redis.' }
    foreach ($verb in 'acquire', 'status', 'renew', 'release', 'ping', 'defer', 'deferred') {
        $arguments = @($verb)
        if ($verb -eq 'defer') { $arguments += @('-Fingerprint', 'offline') }
        $result = Invoke-Worker a $arguments
        if ($result.ExitCode -ne 1 -or $result.Output) { throw "Unavailable backend did not fail closed for $verb." }
    }
    Write-Output 'OK shared Redis: independent caches, competing ownership, renewal, stale release, Unicode, authentication and unavailable backend.'
}
finally {
    & docker rm -f $container 2>$null | Out-Null
    $resolved = [IO.Path]::GetFullPath($fixtureRoot)
    $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
    if (-not $resolved.StartsWith($temp, [StringComparison]::OrdinalIgnoreCase) -or (Split-Path $resolved -Leaf) -ne $container) { throw 'Unsafe fixture cleanup path.' }
    Remove-Item -LiteralPath $resolved -Recurse -Force -ErrorAction SilentlyContinue
}
