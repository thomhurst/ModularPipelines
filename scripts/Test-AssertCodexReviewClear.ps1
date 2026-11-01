# Exercise Assert-CodexReviewClear.ps1 against stubbed issue-comment payloads.
$ErrorActionPreference = 'Stop'

$scriptPath = Join-Path $PSScriptRoot 'Assert-CodexReviewClear.ps1'
$testDirectory = Join-Path ([IO.Path]::GetTempPath()) "modularpipelines-codex-gate-test-$([guid]::NewGuid().ToString('N'))"
$null = New-Item -ItemType Directory -Path $testDirectory
try {
    $head = 'a' * 40
    $other = 'b' * 40

    function New-CodexComment(
        [string]$Sha = $head,
        [string]$Status = 'completed',
        [object]$Unresolved = $false,
        [string]$UpdatedAt = '2026-11-01T00:00:00Z',
        [string]$Login = 'chatgpt-codex-connector[bot]',
        [string]$Security = '{"commit_sha":null,"has_any_findings":null,"has_unresolved_findings":null,"status":null}') {
        $unresolvedJson = if ($null -eq $Unresolved) { 'null' } else { "$Unresolved".ToLowerInvariant() }
        $stateJson = "{`"code_review`":{`"commit_sha`":`"$Sha`",`"has_any_findings`":$unresolvedJson,`"has_unresolved_findings`":$unresolvedJson,`"status`":`"$Status`"},`"security_review`":$Security}"
        @{
            user       = @{ login = $Login }
            updated_at = $UpdatedAt
            body       = "<!-- codex-pull-request-review-summary -->`n<!-- codex-review-state:v1 $stateJson -->`n## Codex Review Summary"
        }
    }

    function Invoke-Gate([object[]]$Comments, [switch]$Slurped) {
        $path = Join-Path $testDirectory 'comments.json'
        $payload = if ($Slurped) { , @($Comments) } else { $Comments }
        ConvertTo-Json -InputObject @($payload) -Depth 10 | Set-Content -LiteralPath $path
        $output = & pwsh -NoProfile -File $scriptPath -Pr 7 -HeadSha $head -CommentsPath $path 2>&1
        [pscustomobject]@{ Exit = $LASTEXITCODE; Output = ($output -join "`n") }
    }

    function Assert-Result($Result, [int]$Exit, [string]$Pattern, [string]$Because) {
        if ($Result.Exit -ne $Exit -or $Result.Output -notmatch $Pattern) {
            throw "$Because (exit $($Result.Exit)): $($Result.Output)"
        }
    }

    Assert-Result (Invoke-Gate @(New-CodexComment)) 0 'clear' 'A completed current-head review without findings must pass'
    Assert-Result (Invoke-Gate @(New-CodexComment) -Slurped) 0 'clear' 'Paginated (--slurp) comment pages must be flattened'
    Assert-Result (Invoke-Gate @(New-CodexComment -Login 'chatgpt-codex-connector')) 0 'clear' 'The GraphQL-style login without [bot] must be accepted'

    Assert-Result (Invoke-Gate @()) 1 'no Codex review state' 'No Codex comment must deny'
    Assert-Result (Invoke-Gate @(New-CodexComment -Login 'someone-else')) 1 'no Codex review state' 'A marker from another author must be ignored'
    Assert-Result (Invoke-Gate @(New-CodexComment -Status 'in_progress')) 1 'status' 'An unfinished review must deny'
    Assert-Result (Invoke-Gate @(New-CodexComment -Sha $other)) 1 'not head' 'A review of an older head must deny'
    Assert-Result (Invoke-Gate @(New-CodexComment -Unresolved $true)) 1 'unresolved findings' 'Unresolved findings must deny'
    Assert-Result (Invoke-Gate @(New-CodexComment -Unresolved $null)) 1 'unresolved findings' 'An unknown findings state must deny'
    Assert-Result (Invoke-Gate @(New-CodexComment -Security '{"commit_sha":null,"has_unresolved_findings":true,"status":"completed"}')) 1 'security' 'Unresolved security findings must deny'

    $stale = New-CodexComment -Unresolved $true -UpdatedAt '2026-11-01T00:00:00Z'
    $fresh = New-CodexComment -UpdatedAt '2026-11-01T01:00:00Z'
    Assert-Result (Invoke-Gate @($fresh, $stale)) 0 'clear' 'The newest Codex state must win regardless of listing order'
    Assert-Result (Invoke-Gate @(
            (New-CodexComment -UpdatedAt '2026-11-01T00:00:00Z'),
            (New-CodexComment -Unresolved $true -UpdatedAt '2026-11-01T01:00:00Z'))) 1 'unresolved findings' 'A newer state with findings must override an older clear state'
}
finally {
    $resolvedTestDirectory = [IO.Path]::GetFullPath($testDirectory)
    $expectedParent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([IO.Path]::DirectorySeparatorChar)
    if ([IO.Path]::GetDirectoryName($resolvedTestDirectory) -ne $expectedParent -or
        [IO.Path]::GetFileName($resolvedTestDirectory) -notlike 'modularpipelines-codex-gate-test-*') {
        throw 'Refusing cleanup outside the temporary Codex gate test directory.'
    }
    Remove-Item -LiteralPath $resolvedTestDirectory -Recurse -Force
}

Write-Output 'OK Assert-CodexReviewClear head binding, newest-state selection, and denial semantics passed.'
