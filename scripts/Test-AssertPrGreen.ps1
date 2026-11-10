#!/usr/bin/env pwsh
# Exercise the complete merge gate with GitHub responses stubbed out.
$ErrorActionPreference = 'Stop'
$scriptPath = Join-Path $PSScriptRoot 'Assert-PrGreen.ps1'
$testDirectory = Join-Path ([IO.Path]::GetTempPath()) "modularpipelines-prgreen-test-$([guid]::NewGuid().ToString('N'))"
$null = New-Item -ItemType Directory -Path $testDirectory
try {
    $harnessPath = Join-Path $testDirectory 'harness.ps1'
    $fixturePath = Join-Path $testDirectory 'fixture.json'
    @'
param([string]$GateScript, [string]$FixturePath, [switch]$ForAutoMerge)
$ErrorActionPreference = 'Stop'
$fixture = Get-Content -LiteralPath $FixturePath -Raw | ConvertFrom-Json
function gh {
    $global:LASTEXITCODE = 0
    if ($args[0] -eq 'pr' -and $args[1] -eq 'view') {
        return ConvertTo-Json -InputObject $fixture.view -Depth 20 -Compress
    }
    if ($args[0] -ne 'api') { throw 'Unexpected GitHub operation.' }
    if ($args -contains 'graphql') {
        if (($args -join ' ') -match 'latestReviews') {
            return ConvertTo-Json -InputObject @(@{ data = @{ repository = @{ pullRequest = @{
                latestReviews = @{ nodes = @($fixture.reviews); pageInfo = @{ hasNextPage = $false } }
            } } } }) -Depth 20 -Compress
        }
        return ConvertTo-Json -InputObject @{ data = @{ repository = @{ pullRequest = @{
            reviewThreads = @{ nodes = @($fixture.threads); pageInfo = @{ hasNextPage = $false } }
        } } } } -Depth 20 -Compress
    }
    $endpoint = @($args | Where-Object { $_ -like 'repos/*' })[0]
    switch -Wildcard ($endpoint) {
        '*/actions/runs/10' { return ConvertTo-Json -InputObject $fixture.cancelledRun -Depth 20 -Compress }
        '*/actions/workflows/claude-code-review.yml/runs?event=workflow_dispatch&*' {
            if ($fixture.apiFailure) { $global:LASTEXITCODE = 1; return }
            return ConvertTo-Json -InputObject @(@{ workflow_runs = @($fixture.recoveryRuns | Where-Object { $_.event -ne 'pull_request_target' }) }) -Depth 20 -Compress
        }
        '*/actions/workflows/claude-code-review.yml/runs?event=pull_request_target&*' {
            return ConvertTo-Json -InputObject @(@{ workflow_runs = @($fixture.recoveryRuns | Where-Object { $_.event -eq 'pull_request_target' }) }) -Depth 20 -Compress
        }
        '*/actions/runs/20/jobs?*' {
            return ConvertTo-Json -InputObject @(@{ jobs = @($fixture.jobs) }) -Depth 20 -Compress
        }
        default { throw "Unexpected GitHub endpoint: $endpoint" }
    }
}
& $GateScript -Pr 123 -Repo owner/repo -ForAutoMerge:$ForAutoMerge -Json
exit $LASTEXITCODE
'@ | Set-Content -LiteralPath $harnessPath

    $headSha = 'a' * 40
    $baseline = @{
        view = @{
            state = 'OPEN'; isDraft = $false; mergeable = 'MERGEABLE'; mergeStateStatus = 'CLEAN'
            headRefOid = $headSha; headRefName = 'issue-123-test'
            commits = @(@{ committedDate = '2026-11-10T08:00:00Z' })
            statusCheckRollup = @(
                @{ name = 'pipeline (ubuntu-latest)'; status = 'COMPLETED'; conclusion = 'SUCCESS' },
                @{ name = 'claude-review'; workflowName = 'Claude Code Review'; status = 'COMPLETED'; conclusion = 'CANCELLED'
                    detailsUrl = 'https://github.com/owner/repo/actions/runs/10/job/11' }
            )
        }
        cancelledRun = @{ workflow_id = 7; conclusion = 'cancelled'; run_started_at = '2026-11-10T09:00:00Z' }
        recoveryRuns = @(@{
            id = 20; workflow_id = 7; path = '.github/workflows/claude-code-review.yml'; name = 'Claude Code Review'
            event = 'workflow_dispatch'; status = 'completed'; conclusion = 'success'
            head_sha = 'b' * 40 # Recovery checks attach to main, not the PR head.
            created_at = '2026-11-10T10:00:00Z'; updated_at = '2026-11-10T10:10:00Z'
        })
        jobs = @(@{ run_id = 20; name = 'claude-review'; status = 'completed'; conclusion = 'success'
            started_at = '2026-11-10T10:01:00Z'; completed_at = '2026-11-10T10:10:00Z' })
        reviews = @(@{
            author = @{ login = 'github-actions' }; state = 'COMMENTED'; commit = @{ oid = $headSha }
            submittedAt = '2026-11-10T10:09:00Z'
            body = "## Claude Review`n`nNo actionable findings.`n<!-- REVIEW_VERDICT: CLEAR HEAD: $headSha -->"
        })
        threads = @()
    }
    $cases = @(
        @{ Name = 'trusted exact-head recovery'; Allows = $true; Change = {} },
        @{ Name = 'trusted pull_request_target recovery'; Allows = $true; Change = { $fixture.recoveryRuns[0].event = 'pull_request_target' } },
        @{ Name = 'healthy head check needs no recovery'; Allows = $true; Change = { $fixture.view.statusCheckRollup[1].conclusion = 'SUCCESS' } },
        @{ Name = 'recovery with actionable-looking headings'; Allows = $true; Change = {
            $fixture.reviews[0].body = "### Risk`nNo action is needed.`n<!-- REVIEW_VERDICT: CLEAR HEAD: $headSha -->"
        } },
        @{ Name = 'failed recovery'; Allows = $false; Change = { $fixture.recoveryRuns[0].conclusion = 'failure' } },
        @{ Name = 'incomplete recovery'; Allows = $false; Change = { $fixture.recoveryRuns[0].status = 'in_progress' } },
        @{ Name = 'failed recovery job'; Allows = $false; Change = { $fixture.jobs[0].conclusion = 'failure' } },
        @{ Name = 'skipped recovery job'; Allows = $false; Change = { $fixture.jobs[0].conclusion = 'skipped' } },
        @{ Name = 'incomplete recovery job'; Allows = $false; Change = { $fixture.jobs[0].status = 'in_progress' } },
        @{ Name = 'wrong-head review'; Allows = $false; Change = { $fixture.reviews[0].commit.oid = 'c' * 40 } },
        @{ Name = 'wrong-head marker'; Allows = $false; Change = { $fixture.reviews[0].body = "<!-- REVIEW_VERDICT: CLEAR HEAD: $('c' * 40) -->" } },
        @{ Name = 'content after clear marker'; Allows = $false; Change = { $fixture.reviews[0].body += "`nMore text" } },
        @{ Name = 'missing review time'; Allows = $false; Change = { $fixture.reviews[0].submittedAt = $null } },
        @{ Name = 'untrusted author'; Allows = $false; Change = { $fixture.reviews[0].author.login = 'someone' } },
        @{ Name = 'legacy Claude author lacks workflow provenance'; Allows = $false; Change = { $fixture.reviews[0].author.login = 'claude' } },
        @{ Name = 'blocking recovery verdict'; Allows = $false; Change = { $fixture.reviews[0].body = "<!-- REVIEW_VERDICT: BLOCKING HEAD: $headSha -->" } },
        @{ Name = 'review outside job interval'; Allows = $false; Change = { $fixture.jobs[0].completed_at = '2026-11-10T10:08:00Z' } },
        @{ Name = 'review outside run interval'; Allows = $false; Change = { $fixture.recoveryRuns[0].updated_at = '2026-11-10T10:08:00Z' } },
        @{ Name = 'different workflow'; Allows = $false; Change = { $fixture.recoveryRuns[0].path = '.github/workflows/other.yml' } },
        @{ Name = 'different workflow ID'; Allows = $false; Change = { $fixture.cancelledRun.workflow_id = 8 } },
        @{ Name = 'untrusted event'; Allows = $false; Change = { $fixture.recoveryRuns[0].event = 'pull_request' } },
        @{ Name = 'wrong job run'; Allows = $false; Change = { $fixture.jobs[0].run_id = 30 } },
        @{ Name = 'older recovery'; Allows = $false; Change = { $fixture.cancelledRun.run_started_at = '2026-11-10T10:15:00Z' } },
        @{ Name = 'missing cancelled run time'; Allows = $false; Change = { $fixture.cancelledRun.run_started_at = $null } },
        @{ Name = 'missing workflow identity'; Allows = $false; Change = {
            $fixture.cancelledRun.workflow_id = $null; $fixture.recoveryRuns[0].workflow_id = $null
        } },
        @{ Name = 'missing run URL'; Allows = $false; Change = { $fixture.view.statusCheckRollup[1].detailsUrl = $null } },
        @{ Name = 'missing workflow name'; Allows = $false; Change = { $fixture.view.statusCheckRollup[1].workflowName = $null } },
        @{ Name = 'unrelated cancelled check'; Allows = $false; Change = { $fixture.view.statusCheckRollup[1].name = 'other-check' } },
        @{ Name = 'failed head review check'; Allows = $false; Change = { $fixture.view.statusCheckRollup[1].conclusion = 'FAILURE' } },
        @{ Name = 'run not cancelled'; Allows = $false; Change = { $fixture.cancelledRun.conclusion = 'failure' } },
        @{ Name = 'unrelated failed check'; Allows = $false; Change = { $fixture.view.statusCheckRollup[0].conclusion = 'FAILURE' } },
        @{ Name = 'unrelated cancelled check remains'; Allows = $false; Change = {
            $fixture.view.statusCheckRollup += @{ name = 'other-check'; status = 'COMPLETED'; conclusion = 'CANCELLED' }
        } },
        @{ Name = 'pending review check remains'; Allows = $false; Change = {
            $fixture.view.statusCheckRollup += @{ name = 'claude-review'; status = 'IN_PROGRESS'; conclusion = $null }
        } },
        @{ Name = 'unresolved thread'; Allows = $false; Change = { $fixture.threads = @(@{ isResolved = $false }) } },
        @{ Name = 'changes requested'; Allows = $false; Change = {
            $fixture.reviews += @{ state = 'CHANGES_REQUESTED'; author = @{ login = 'reviewer' } }
        } },
        @{ Name = 'other actionable review'; Allows = $false; Change = {
            $fixture.reviews += @{ state = 'COMMENTED'; author = @{ login = 'reviewer' }; body = '### Bug: data loss' }
        } },
        @{ Name = 'review API failure'; Allows = $false; Change = { $fixture.apiFailure = $true } },
        @{ Name = 'no recovery runs'; Allows = $false; Change = { $fixture.recoveryRuns = @() } },
        @{ Name = 'no recovery review'; Allows = $false; Change = { $fixture.reviews = @() } }
    )
    foreach ($case in $cases) {
        foreach ($autoMerge in @($false, $true)) {
            $fixture = $baseline | ConvertTo-Json -Depth 20 | ConvertFrom-Json -AsHashtable
            & $case.Change
            $fixture | ConvertTo-Json -Depth 20 | Set-Content -LiteralPath $fixturePath
            $output = & pwsh -NoProfile -File $harnessPath -GateScript $scriptPath -FixturePath $fixturePath `
                -ForAutoMerge:$autoMerge 2>&1
            $exitCode = $LASTEXITCODE
            $expectedExit = if ($case.Allows) { 0 } else { 1 }
            if ($exitCode -ne $expectedExit) {
                throw "$($case.Name) (auto-merge=$autoMerge): expected exit $expectedExit, got ${exitCode}: $($output -join '`n')"
            }
            if (-not $case.Allows -and ($output -join "`n") -notmatch 'DO NOT MERGE #123 --') {
                throw "$($case.Name) must fail with a gate denial: $($output -join '`n')"
            }
            if ($case.Allows -and ($output | ConvertFrom-Json).headRefOid -ne $headSha) {
                throw 'The gate must return the reviewed PR head, not the recovery workflow base SHA.'
            }
        }
    }
}
finally {
    $resolved = [IO.Path]::GetFullPath($testDirectory)
    $parent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([IO.Path]::DirectorySeparatorChar)
    if ([IO.Path]::GetDirectoryName($resolved) -ne $parent -or
        [IO.Path]::GetFileName($resolved) -notlike 'modularpipelines-prgreen-test-*') {
        throw 'Refusing cleanup outside the temporary merge gate test directory.'
    }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
Write-Output "OK merge gate recovery tests passed ($($cases.Count) cases in merge and auto-merge modes)."
$global:LASTEXITCODE = 0
