$ErrorActionPreference = 'Stop'
$resolver = Join-Path $PSScriptRoot 'Resolve-DistributedBuildMatrix.ps1'
foreach ($endpoint in @('', ' ', 'private-endpoint')) {
    foreach ($key in @('', ' ', 'private-key')) {
        foreach ($allow in @('true', 'false')) {
            $result = & $resolver -RedisEndpoint $endpoint -RedisKey $key -AllowDistributed $allow -RunId 123 -RunAttempt 2
            $distributed = $allow -eq 'true' -and $endpoint.Trim() -ne '' -and $key.Trim() -ne ''
            $matrix = $result.matrix | ConvertFrom-Json
            $expectedCount = if ($distributed) { 4 } else { 1 }
            if ($result.distributed -ne $distributed.ToString().ToLowerInvariant() -or
                $matrix.include.Count -ne $expectedCount -or $result.run_identifier -ne '123-2') {
                throw 'Incorrect secretless/trusted matrix routing.'
            }

            if ($matrix.include[0].instance -ne 0 -or $matrix.include[0].os -ne 'ubuntu-latest' -or
                @($matrix.include | Where-Object total -ne $expectedCount).Count -ne 0) {
                throw 'The primary or shared instance count is invalid.'
            }

            if ($distributed -and (@($matrix.include.os) -join ',') -ne 'ubuntu-latest,ubuntu-latest,windows-latest,macos-latest') {
                throw 'Distributed mode lost a required operating system.'
            }

            if (($result | ConvertTo-Json -Depth 4) -match 'private-') { throw 'Matrix output exposes credentials.' }
        }
    }
}

foreach ($attempt in @('', '0', '-1', 'not-an-attempt')) {
    $rejected = $false
    try { & $resolver -RunId 123 -RunAttempt $attempt | Out-Null } catch { $rejected = $true }
    if (-not $rejected) { throw 'Invalid run attempt was accepted.' }
}
Write-Output 'Distributed matrix routing: 22 cases passed.'
