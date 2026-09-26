[CmdletBinding()]
param(
    [string]$RedisEndpoint = $env:REDIS_ENDPOINT,
    [string]$RedisKey = $env:REDIS_KEY,
    [string]$AllowDistributed = $env:ALLOW_DISTRIBUTED,
    [string]$RunId = $env:GITHUB_RUN_ID,
    [string]$RunAttempt = $env:GITHUB_RUN_ATTEMPT,
    [string]$GitHubOutput
)

$ErrorActionPreference = 'Stop'
if ($RunId -notmatch '^\d+$' -or $RunAttempt -notmatch '^[1-9]\d*$') {
    throw 'A GitHub run ID and positive attempt number are required.'
}

$distributed = $AllowDistributed -eq 'true' -and
    -not [string]::IsNullOrWhiteSpace($RedisEndpoint) -and
    -not [string]::IsNullOrWhiteSpace($RedisKey)
$runners = @(if ($distributed) { @('ubuntu-latest', 'ubuntu-latest', 'windows-latest', 'macos-latest') } else { @('ubuntu-latest') })
$instances = @(for ($index = 0; $index -lt $runners.Count; $index++) {
    @{ instance = $index; os = $runners[$index]; total = $runners.Count }
})
$result = [pscustomobject]@{
    distributed = $distributed.ToString().ToLowerInvariant()
    run_identifier = "$RunId-$RunAttempt"
    matrix = @{ include = @($instances | Where-Object instance -eq 0) } | ConvertTo-Json -Depth 3 -Compress
    worker_matrix = @{ include = @($instances | Where-Object instance -ne 0) } | ConvertTo-Json -Depth 3 -Compress
}

if ($GitHubOutput) {
    @("distributed=$($result.distributed)", "run_identifier=$($result.run_identifier)",
        "matrix=$($result.matrix)", "worker_matrix=$($result.worker_matrix)") |
        Add-Content -LiteralPath $GitHubOutput -Encoding utf8
} else {
    $result
}
