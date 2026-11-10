#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

$repositoryRoot = Split-Path $PSScriptRoot -Parent
$assertScript = Join-Path $PSScriptRoot 'Assert-RequiredPipelineContext.ps1'

# Only the selected validation route may pass. Missing routing outputs must fail closed.
$caseCount = 0
foreach ($runFull in @('true', 'false', '', 'invalid')) {
    foreach ($generated in @('true', 'false', '', 'invalid')) {
        foreach ($fastResult in @('success', 'failure', 'cancelled', 'skipped')) {
            foreach ($fullResult in @('success', 'failure', 'cancelled', 'skipped')) {
                foreach ($generatedResult in @('success', 'failure', 'cancelled', 'skipped')) {
                    $shouldPass = $fastResult -eq 'success' -and (
                        ($runFull -eq 'true' -and $generated -eq 'false' -and
                            $fullResult -eq 'success' -and $generatedResult -eq 'skipped') -or
                        ($runFull -eq 'false' -and $generated -eq 'true' -and
                            $fullResult -eq 'skipped' -and $generatedResult -eq 'success') -or
                        ($runFull -eq 'false' -and $generated -eq 'false' -and
                            $fullResult -eq 'skipped' -and $generatedResult -eq 'skipped'))
                    $crossPlatformResult = if ($runFull -eq 'true') { 'success' } else { 'skipped' }
                    $passed = $true
                    try {
                        & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration $generated `
                            -FastFailResult $fastResult -FullPipelineResult $fullResult `
                            -GeneratedIntegrationResult $generatedResult -CrossPlatformBuildResult $crossPlatformResult `
                            -ArtifactStorageResult $crossPlatformResult `
                            -AnalyzersResult $crossPlatformResult -TrimAotResult $crossPlatformResult `
                            -RunIntegrationValidation 'false' -IntegrationResult 'skipped' *> $null
                    }
                    catch {
                        $passed = $false
                    }
                    if ($passed -ne $shouldPass) {
                        throw "Unexpected route result: full=$runFull generated=$generated fast=$fastResult fullResult=$fullResult generatedResult=$generatedResult"
                    }
                    $caseCount++
                }
            }
        }
    }
}
Write-Host "Required pipeline routing: $caseCount cases passed."

$runnerCaseCount = 0
foreach ($runFull in @('true', 'false')) {
    foreach ($distributed in @('true', 'false', '', 'invalid')) {
        foreach ($workerResult in @('success', 'failure', 'cancelled', 'skipped')) {
            foreach ($crossPlatformResult in @('success', 'failure', 'cancelled', 'skipped')) {
                $fullResult = if ($runFull -eq 'true') { 'success' } else { 'skipped' }
                $shouldPass = ($runFull -eq 'false' -and $workerResult -eq 'skipped' -and $crossPlatformResult -eq 'skipped') -or
                    ($runFull -eq 'true' -and $distributed -eq 'true' -and $workerResult -eq 'success' -and $crossPlatformResult -eq 'skipped') -or
                    ($runFull -eq 'true' -and $distributed -eq 'false' -and $workerResult -eq 'skipped' -and $crossPlatformResult -eq 'success')
                $passed = $true
                try {
                    & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration 'false' `
                        -FastFailResult 'success' -FullPipelineResult $fullResult -GeneratedIntegrationResult 'skipped' `
                        -Distributed $distributed -WorkerPipelineResult $workerResult `
                        -CrossPlatformBuildResult $crossPlatformResult `
                        -ArtifactStorageResult $fullResult `
                        -AnalyzersResult $fullResult -TrimAotResult $fullResult `
                        -RunIntegrationValidation 'false' -IntegrationResult 'skipped' *> $null
                }
                catch {
                    $passed = $false
                }
                if ($passed -ne $shouldPass) {
                    throw "Unexpected runner route: full=$runFull distributed=$distributed workers=$workerResult crossPlatform=$crossPlatformResult"
                }
                $runnerCaseCount++
            }
        }
    }
}
Write-Host "Required runner routing: $runnerCaseCount cases passed."

$integrationCaseCount = 0
foreach ($runIntegration in @('true', 'false', '', 'invalid')) {
    foreach ($integrationResult in @('success', 'failure', 'cancelled', 'skipped')) {
        foreach ($route in @('full', 'generated', 'fast-only')) {
            $full = $route -eq 'full'
            $generated = $route -eq 'generated'
            $expectedResult = if ($runIntegration -eq 'true') { 'success' } else { 'skipped' }
            $shouldPass = $runIntegration -in @('true', 'false') -and
                -not ($generated -and $runIntegration -eq 'true') -and $integrationResult -eq $expectedResult
            $passed = $true
            try {
                & $assertScript -RunFullPipeline $full.ToString().ToLowerInvariant() `
                    -IsGeneratedIntegration $generated.ToString().ToLowerInvariant() `
                    -FastFailResult success -FullPipelineResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -CrossPlatformBuildResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -ArtifactStorageResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -AnalyzersResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -TrimAotResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -GeneratedIntegrationResult $(if ($generated) { 'success' } else { 'skipped' }) `
                    -RunIntegrationValidation $runIntegration -IntegrationResult $integrationResult *> $null
            }
            catch { $passed = $false }
            if ($passed -ne $shouldPass) {
                throw "Unexpected integration gate: route=$route required=$runIntegration result=$integrationResult."
            }
            $integrationCaseCount++
        }
    }
}
Write-Host "Required integration routing: $integrationCaseCount cases passed."

foreach ($runFull in @('true', 'false')) {
    foreach ($storageResult in @('success', 'failure', 'cancelled', 'skipped')) {
        $expected = if ($runFull -eq 'true') { 'success' } else { 'skipped' }
        $passed = $true
        try {
            & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration 'false' `
                -FastFailResult success -FullPipelineResult $expected -GeneratedIntegrationResult skipped `
                -CrossPlatformBuildResult $expected -ArtifactStorageResult $storageResult `
                -AnalyzersResult $expected -TrimAotResult $expected `
                -RunIntegrationValidation false -IntegrationResult skipped *> $null
        }
        catch { $passed = $false }
        if ($passed -ne ($storageResult -eq $expected)) {
            throw "Unexpected artifact storage gate: full=$runFull storage=$storageResult."
        }
    }
}

$qualityCaseCount = 0
foreach ($route in @('full', 'distributed', 'generated', 'integration', 'fast-only')) {
    foreach ($analyzersResult in @('success', 'failure', 'cancelled', 'skipped', '', 'invalid')) {
        foreach ($trimAotResult in @('success', 'failure', 'cancelled', 'skipped', '', 'invalid')) {
            $runFull = $route -in @('full', 'distributed')
            $expected = if ($runFull) { 'success' } else { 'skipped' }
            $passed = $true
            try {
                & $assertScript -RunFullPipeline $runFull.ToString().ToLowerInvariant() `
                    -IsGeneratedIntegration ($route -eq 'generated').ToString().ToLowerInvariant() `
                    -FastFailResult success -FullPipelineResult $expected `
                    -Distributed ($route -eq 'distributed').ToString().ToLowerInvariant() `
                    -WorkerPipelineResult $(if ($route -eq 'distributed') { 'success' } else { 'skipped' }) `
                    -CrossPlatformBuildResult $(if ($route -eq 'full') { 'success' } else { 'skipped' }) `
                    -ArtifactStorageResult $expected `
                    -GeneratedIntegrationResult $(if ($route -eq 'generated') { 'success' } else { 'skipped' }) `
                    -RunIntegrationValidation ($route -eq 'integration').ToString().ToLowerInvariant() `
                    -IntegrationResult $(if ($route -eq 'integration') { 'success' } else { 'skipped' }) `
                    -AnalyzersResult $analyzersResult -TrimAotResult $trimAotResult *> $null
            }
            catch { $passed = $false }
            if ($passed -ne ($analyzersResult -eq $expected -and $trimAotResult -eq $expected)) {
                throw "Unexpected quality gate: route=$route analyzers=$analyzersResult trimAot=$trimAotResult."
            }
            $qualityCaseCount++
        }
    }
}
Write-Host "Required analyzer and trim/AOT routing: $qualityCaseCount cases passed."

# Each supplemental workflow is required exactly when its path route selects it.
$supplementalCaseCount = 0
foreach ($validation in @('DistributedTemplate', 'GoValidation', 'DocsDeploy')) {
    foreach ($required in @('true', 'false', '', 'invalid')) {
        foreach ($result in @('success', 'failure', 'cancelled', 'skipped', '', 'invalid')) {
            $arguments = @{
                RunFullPipeline = 'false'; IsGeneratedIntegration = 'false'
                FastFailResult = 'success'; FullPipelineResult = 'skipped'
                GeneratedIntegrationResult = 'skipped'; RunIntegrationValidation = 'false'
                IntegrationResult = 'skipped'; AnalyzersResult = 'skipped'; TrimAotResult = 'skipped'
            }
            $arguments["Run$validation"] = $required
            $arguments["${validation}Result"] = $result
            $passed = $true
            try { & $assertScript @arguments *> $null } catch { $passed = $false }
            $expected = if ($required -eq 'true') { 'success' } else { 'skipped' }
            $shouldPass = $required -in @('true', 'false') -and $result -eq $expected
            if ($passed -ne $shouldPass) {
                throw "Unexpected supplemental gate: validation=$validation required=$required result=$result."
            }
            $supplementalCaseCount++
        }
    }
}
Write-Host "Required supplemental routing: $supplementalCaseCount cases passed."

$workflow = Get-Content -LiteralPath (Join-Path $repositoryRoot '.github/workflows/dotnet.yml') -Raw
$requiredJob = [regex]::Match(
    $workflow,
    '(?ms)^  required-pipeline:.*?(?=^  [a-z0-9-]+:|\z)').Value
if ([string]::IsNullOrWhiteSpace($requiredJob)) {
    throw 'Required pipeline aggregate job was not found.'
}

foreach ($requiredText in @(
             # A job skipped by `if:` reports success under its literal name, which would
             # satisfy branch protection on a draft. A matrix name stays unexpanded
             # (`pipeline (${{ matrix.os }})`) when skipped, so only a real run reports it.
             'name: pipeline (${{ matrix.os }})',
             'os: [ubuntu-latest]',
             'runs-on: ${{ matrix.os }}',
             'needs: [fast-fail, pipeline, pipeline-workers, cross-platform-build, generated-integration, changed-integration, artifact-storage, analyzers, trim-aot, cmd-tests, distributed-template, go-validation, docs-deploy]',
             "if: always() && (github.event_name != 'pull_request' || github.event.pull_request.draft == false)",
             '-RunFullPipeline ''${{ needs.fast-fail.outputs.run_full_pipeline }}''',
             '-Distributed ''${{ needs.fast-fail.outputs.distributed }}''',
             '-WorkerPipelineResult ''${{ needs.pipeline-workers.result }}''',
             '-CrossPlatformBuildResult ''${{ needs.cross-platform-build.result }}''',
             '-ArtifactStorageResult ''${{ needs.artifact-storage.result }}''',
             '-AnalyzersResult ''${{ needs.analyzers.result }}''',
             '-TrimAotResult ''${{ needs.trim-aot.result }}''',
             '-RunIntegrationValidation ''${{ needs.fast-fail.outputs.run_integration_validation }}''',
             '-IntegrationResult ''${{ needs.changed-integration.result }}''',
             '-RunDistributedTemplate ''${{ needs.fast-fail.outputs.run_distributed_template }}''',
             '-DistributedTemplateResult ''${{ needs.distributed-template.result }}''',
             '-RunGoValidation ''${{ needs.fast-fail.outputs.run_go_validation }}''',
             '-GoValidationResult ''${{ needs.go-validation.result }}''',
             '-RunDocsDeploy ''${{ needs.fast-fail.outputs.run_docs_deploy }}''',
             '-DocsDeployResult ''${{ needs.docs-deploy.result }}''',
             './scripts/Assert-RequiredPipelineContext.ps1'
         )) {
    if (-not $requiredJob.Contains($requiredText, [StringComparison]::Ordinal)) {
        throw "Required pipeline aggregate omitted '$requiredText'."
    }
}

# Drafts are reviewed before CI: every job depends on fast-fail, so gating it skips the
# workflow before any build runner starts, and readiness starts the run.
$pullRequestTrigger = [regex]::Match($workflow, '(?ms)^  pull_request:.*?(?=^  [a-z_]+:)').Value
if (-not $pullRequestTrigger.Contains(
        'types: [opened, synchronize, reopened, ready_for_review, converted_to_draft]',
        [StringComparison]::Ordinal)) {
    throw 'Pull request CI must start when a draft becomes ready and cancel when it returns to draft.'
}
$fastFailJob = [regex]::Match(
    $workflow,
    '(?ms)^  fast-fail:.*?(?=^  [a-z0-9-]+:)').Value
if (-not $fastFailJob.Contains(
        "if: github.event_name != 'pull_request' || github.event.pull_request.draft == false",
        [StringComparison]::Ordinal)) {
    throw 'Fast fail must skip draft pull requests before allocating a runner.'
}
$jobsSection = $workflow.Substring([regex]::Match($workflow, '(?m)^jobs:').Index)
foreach ($job in [regex]::Matches($jobsSection, '(?ms)^  ([a-z0-9-]+):\r?\n(.*?)(?=^  [a-z0-9-]+:|\z)')) {
    $name = $job.Groups[1].Value
    if ($name -eq 'fast-fail') { continue }
    if ($job.Groups[2].Value -notmatch '(?m)^    needs: \[?[^\]\r\n]*\b(fast-fail|required-pipeline)\b') {
        throw "Job '$name' must depend on fast-fail (directly or through required-pipeline) so drafts skip it."
    }
}

$fullPipelineJob = [regex]::Match(
    $workflow,
    '(?ms)^  pipeline:.*?(?=^  [a-z0-9-]+:)').Value
if (-not $fullPipelineJob.Contains(
        'name: full pipeline (${{ matrix.os }}, instance ${{ matrix.instance }})',
        [StringComparison]::Ordinal)) {
    throw 'The full pipeline job must not emit the required aggregate context directly.'
}

$workerJob = [regex]::Match(
    $workflow,
    '(?ms)^  pipeline-workers:.*?(?=^  [a-z0-9-]+:)').Value
if ($workerJob -notmatch '(?m)^    permissions:\s*\r?\n      contents: read\s*\r?\n    timeout-minutes:' -or
    -not $workerJob.Contains('steps: *pipeline-steps', [StringComparison]::Ordinal) -or
    -not $workerJob.Contains("needs.fast-fail.outputs.distributed == 'true'", [StringComparison]::Ordinal)) {
    throw 'Distributed workers must share pipeline steps with read-only repository permissions and no identity-token grant.'
}
if (-not $fullPipelineJob.Contains('steps: &pipeline-steps', [StringComparison]::Ordinal) -or
    $fullPipelineJob.Contains('id-token: write', [StringComparison]::Ordinal) -or
    $fullPipelineJob.Contains('contents: write', [StringComparison]::Ordinal)) {
    throw 'The master must retain the shared steps with read-only validation permissions.'
}

$cmdJob = [regex]::Match($workflow, '(?ms)^  cmd-tests:.*?(?=^  [a-z0-9-]+:|\z)').Value
if (-not $cmdJob.Contains('runs-on: windows-latest', [StringComparison]::Ordinal) -or
    -not $cmdJob.Contains('test/ModularPipelines.Cmd.UnitTests/ModularPipelines.Cmd.UnitTests.csproj', [StringComparison]::Ordinal) -or
    -not $cmdJob.Contains("needs.fast-fail.outputs.run_full_pipeline == 'true'", [StringComparison]::Ordinal)) {
    throw 'Cmd regression tests must run on Windows for full validation.'
}

# Exercise the actual aggregate step so a failed or skipped Cmd run cannot pass the gate.
$cmdGate = [regex]::Match($requiredJob,
    '(?ms)      - name: Validate Cmd test result.*?        run: \|\r?\n(?<script>.*?)(?=      - name:)')
if (-not $cmdGate.Success) { throw 'Cmd result gate was not found.' }
$cmdGateScript = [scriptblock]::Create(($cmdGate.Groups['script'].Value -replace '(?m)^          ', ''))
$originalFull = $env:RUN_FULL_PIPELINE
$originalCmdResult = $env:CMD_TEST_RESULT
try {
    foreach ($full in @('true', 'false')) {
        foreach ($result in @('success', 'failure', 'cancelled', 'skipped')) {
            $env:RUN_FULL_PIPELINE = $full
            $env:CMD_TEST_RESULT = $result
            $passed = $true
            try { & $cmdGateScript } catch { $passed = $false }
            $expected = if ($full -eq 'true') { 'success' } else { 'skipped' }
            if ($passed -ne ($result -eq $expected)) {
                throw "Unexpected Cmd gate result for full=$full and result=$result."
            }
        }
    }
}
finally {
    $env:RUN_FULL_PIPELINE = $originalFull
    $env:CMD_TEST_RESULT = $originalCmdResult
}
Write-Host 'Required pipeline context tests passed.'

# Supplemental CI must run under the required aggregate, with no duplicate PR triggers
# or reusable concurrency group that cancels its caller.
foreach ($route in @(
        @{ Job = 'distributed-template'; Output = 'run_distributed_template'; Workflow = 'distributed-template-smoke' },
        @{ Job = 'go-validation'; Output = 'run_go_validation'; Workflow = 'go-validation' },
        @{ Job = 'docs-deploy'; Output = 'run_docs_deploy'; Workflow = 'deploy-pages-test' }
    )) {
    $job = [regex]::Match($workflow, "(?ms)^  $($route.Job):.*?(?=^  [a-z0-9-]+:|\z)").Value
    foreach ($text in @(
            'needs: fast-fail',
            "if: needs.fast-fail.outputs.$($route.Output) == 'true'",
            "uses: ./.github/workflows/$($route.Workflow).yml"
        )) {
        if (-not $job.Contains($text, [StringComparison]::Ordinal)) {
            throw "Supplemental job '$($route.Job)' omitted '$text'."
        }
    }
    $output = "$($route.Output): " + '${{ steps.full_pipeline.outputs.' + $route.Output + ' }}'
    if (-not $fastFailJob.Contains($output, [StringComparison]::Ordinal)) {
        throw "Fast fail must expose '$($route.Output)' to the caller and aggregate."
    }
    $called = Get-Content (Join-Path $repositoryRoot ".github/workflows/$($route.Workflow).yml") -Raw
    if ($called -notmatch '(?m)^  workflow_call:' -or $called -match '(?m)^  (pull_request|push):') {
        throw "Supplemental workflow '$($route.Workflow)' must be called only through the required pipeline or manual dispatch."
    }
    if ($called.Contains('group: ${{ github.workflow }}', [StringComparison]::Ordinal)) {
        throw "Reusable workflow '$($route.Workflow)' must not share the caller concurrency group."
    }
}
Write-Host 'Supplemental workflow wiring tests passed.'

# Execute the reusable smoke workflow's final assertion so an always-running
# identity summary cannot conceal failed initialization or matrix workers.
$smokeWorkflow = Get-Content (Join-Path $repositoryRoot '.github/workflows/distributed-template-smoke.yml') -Raw
$identities = [regex]::Match($smokeWorkflow, '(?ms)^  identities:.*?(?=^  [a-z0-9-]+:|\z)').Value
if (-not $identities.Contains('needs: [initialize, pipeline]', [StringComparison]::Ordinal)) {
    throw 'The smoke assertion must observe both initialization and matrix workers.'
}
$smokeGate = [regex]::Match($identities, '(?ms)      - name: Validate smoke results.*?        run: \|\r?\n(?<script>.*)')
if (-not $smokeGate.Success) { throw 'Smoke result gate was not found.' }
foreach ($text in @(
        'INITIALIZE_RESULT: ${{ needs.initialize.result }}',
        'PIPELINE_RESULT: ${{ needs.pipeline.result }}'
    )) {
    if (-not $smokeGate.Value.Contains($text, [StringComparison]::Ordinal)) {
        throw "Smoke result gate omitted '$text'."
    }
}
$smokeGateScript = [scriptblock]::Create(($smokeGate.Groups['script'].Value -replace '(?m)^          ', ''))
$originalInitialize = $env:INITIALIZE_RESULT
$originalPipeline = $env:PIPELINE_RESULT
try {
    foreach ($initialize in @('success', 'failure', 'cancelled', 'skipped', '')) {
        foreach ($pipeline in @('success', 'failure', 'cancelled', 'skipped', '')) {
            $env:INITIALIZE_RESULT = $initialize
            $env:PIPELINE_RESULT = $pipeline
            $passed = $true
            try { & $smokeGateScript } catch { $passed = $false }
            if ($passed -ne ($initialize -eq 'success' -and $pipeline -eq 'success')) {
                throw "Unexpected smoke result: initialize=$initialize pipeline=$pipeline."
            }
        }
    }
}
finally {
    $env:INITIALIZE_RESULT = $originalInitialize
    $env:PIPELINE_RESULT = $originalPipeline
}
Write-Host 'Reusable smoke result tests passed.'
