$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'Resolve-GeneratedIntegrationValidation.ps1') `
    -HeadRef ignored `
    -ChangedPath @()

function Invoke-Git {
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [Parameter(ValueFromRemainingArguments)][string[]]$ArgumentList
    )

    & git -C $RepositoryRoot @ArgumentList | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "git $($ArgumentList -join ' ') failed."
    }
}

function Assert-GeneratedBaselineFreshness {
    param([Parameter(Mandatory)][string]$RepositoryRoot)

    $route = Resolve-GeneratedIntegrationValidation `
        -HeadRef 'automated/update-cli-options-fake' `
        -HeadRepository 'owner/repo' `
        -PullRequestAuthor 'owner' `
        -Repository 'owner/repo' `
        -ChangedPath @('src/ModularPipelines.Fake/PublicAPI.Shipped.txt') `
        -RepositoryRoot $RepositoryRoot
    if (-not $route.IsGeneratedIntegration) {
        throw "Shipped baseline changes bypassed the freshness gate: $($route.Reason)"
    }

    & (Join-Path $PSScriptRoot 'Assert-GeneratedOptionsFreshness.ps1') `
        -RepositoryRoot $RepositoryRoot `
        -Tool $route.Tool `
        -Package $route.Package `
        -NamespacePrefix $route.NamespacePrefix `
        -CurrentBase HEAD
}

$repositoryRoot = Split-Path $PSScriptRoot -Parent
$tempRoot = Join-Path ([IO.Path]::GetTempPath()) "generated-options-provenance-$([Guid]::NewGuid().ToString('N'))"
$writeScript = Join-Path $PSScriptRoot 'Write-GeneratedOptionsProvenance.ps1'
$assertScript = Join-Path $PSScriptRoot 'Assert-GeneratedOptionsFreshness.ps1'
$provenanceFunctions = Join-Path $PSScriptRoot 'GeneratedOptionsProvenance.ps1'
$generationWorkflow = Join-Path $repositoryRoot '.github/workflows/generate-cli-options.yml'
$stageScript = Join-Path `
    $repositoryRoot `
    'tools/ModularPipelines.OptionsGenerator/scripts/Stage-GeneratedChanges.ps1'
$changeManifest = "$tempRoot.manifest"

try {
    . $provenanceFunctions
    $sourcePaths = @(Get-GeneratedOptionsSourcePath)
    $buildInputs = @(
        'Directory.Build.props',
        'Directory.Packages.props',
        'global.json',
        'tools/Directory.Build.props'
    )
    foreach ($buildInput in $buildInputs) {
        if ($sourcePaths -notcontains $buildInput) {
            throw "Generated-options fingerprint omits build input '$buildInput'."
        }
    }

    $workflowContents = Get-Content -LiteralPath $generationWorkflow -Raw
    $normalizedWorkflowContents = $workflowContents.ReplaceLineEndings("`n")
    foreach ($buildInput in $buildInputs) {
        if (-not $workflowContents.Contains("- '$buildInput'", [StringComparison]::Ordinal)) {
            throw "Generator push trigger omits build input '$buildInput'."
        }
    }
    $provenanceInputs = @(
        'scripts/GeneratedOptionsProvenance.ps1',
        'scripts/Write-GeneratedOptionsProvenance.ps1'
    )
    foreach ($provenanceInput in $provenanceInputs) {
        if ($sourcePaths -notcontains $provenanceInput) {
            throw "Generated-options fingerprint omits provenance input '$provenanceInput'."
        }

        if (-not $workflowContents.Contains("- '$provenanceInput'", [StringComparison]::Ordinal)) {
            throw "Generator push trigger omits provenance input '$provenanceInput'."
        }
    }
    $guardedAutoMergeDisableSteps = [regex]::Matches(
        $workflowContents,
        "(?ms)^      - name: Disable inherited auto-merge for push refreshes`r?`n" +
        "(?:(?!^      - name:).)*?gh pr list" +
        "(?:(?!^      - name:).)*?--json autoMergeRequest" +
        "(?:(?!^      - name:).)*?\.autoMergeRequest != null" +
        "(?:(?!^      - name:).)*?--disable-auto")
    if ($guardedAutoMergeDisableSteps.Count -ne 2) {
        throw 'Linux and Windows generation jobs must each guard auto-merge disabling on the current PR state.'
    }
    $linuxJobStart = $normalizedWorkflowContents.IndexOf("  generate:`n", [StringComparison]::Ordinal)
    $windowsJobStart = $normalizedWorkflowContents.IndexOf(
        "  generate-windows:`n",
        [StringComparison]::Ordinal)
    $reportJobStart = $normalizedWorkflowContents.IndexOf(
        "  report-scheduled-failure:`n",
        [StringComparison]::Ordinal)
    if ($linuxJobStart -lt 0 -or $windowsJobStart -le $linuxJobStart -or
        $reportJobStart -le $windowsJobStart) {
        throw 'Could not resolve generation job boundaries.'
    }

    $linuxJob = $normalizedWorkflowContents.Substring(
        $linuxJobStart,
        $windowsJobStart - $linuxJobStart)
    $windowsJob = $normalizedWorkflowContents.Substring(
        $windowsJobStart,
        $reportJobStart - $windowsJobStart)
    if ($windowsJob -notmatch "(?ms)gh pr merge \`$prNumber --disable-auto`n\s+if \(\`$LASTEXITCODE -ne 0\)") {
        throw 'Windows generation must fail when disabling inherited auto-merge fails.'
    }
    $expectedConcurrencyGroup = "group: `${{ github.workflow }}-generated-refresh-`${{ matrix.tool }}"
    foreach ($job in @($linuxJob, $windowsJob)) {
        $disableIndex = $job.IndexOf(
            '- name: Disable inherited auto-merge for push refreshes',
            [StringComparison]::Ordinal)
        $buildIndex = $job.IndexOf('- name: Build Generator', [StringComparison]::Ordinal)
        if ($disableIndex -lt 0 -or $buildIndex -lt 0 -or $disableIndex -gt $buildIndex) {
            throw 'Push refreshes must disable inherited auto-merge before generator work begins.'
        }

        if (-not $job.Contains($expectedConcurrencyGroup, [StringComparison]::Ordinal) -or
            -not $job.Contains('cancel-in-progress: false', [StringComparison]::Ordinal) -or
            -not $job.Contains('queue: max', [StringComparison]::Ordinal)) {
            throw 'Generated refresh writes must queue and serialize per tool without cancelling active work.'
        }
    }
    if ($normalizedWorkflowContents -match '(?m)^concurrency:') {
        throw 'Workflow-level concurrency can evict unrelated pending full-refresh jobs.'
    }

    New-Item -ItemType Directory -Path $tempRoot | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tempRoot '.github/workflows') | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tempRoot 'tools/ModularPipelines.OptionsGenerator') | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tempRoot 'src/ModularPipelines.Fake/Generated') | Out-Null
    Set-Content `
        -LiteralPath (Join-Path $tempRoot 'src/ModularPipelines.Fake/ModularPipelines.Fake.slnx') `
        -Value '<Solution />'
    Set-Content `
        -LiteralPath (Join-Path $tempRoot 'src/ModularPipelines.Fake/PublicAPI.Shipped.txt') `
        -Value '#nullable enable'

    foreach ($sourcePath in Get-GeneratedOptionsSourcePath) {
        if ($sourcePath -eq 'tools/ModularPipelines.OptionsGenerator') {
            continue
        }

        $fullSourcePath = Join-Path $tempRoot $sourcePath
        New-Item -ItemType Directory -Path (Split-Path $fullSourcePath -Parent) -Force | Out-Null
        Set-Content -LiteralPath $fullSourcePath -Value "source: $sourcePath"
    }

    Set-Content `
        -LiteralPath (Join-Path $tempRoot '.github/workflows/generate-cli-options.yml') `
        -Value 'name: Generate CLI Options'
    Set-Content `
        -LiteralPath (Join-Path $tempRoot 'tools/ModularPipelines.OptionsGenerator/Emitter.cs') `
        -Value 'using Legacy.CommandContext;'
    [ordered]@{
        formatVersion = 1
        toolName = 'fake'
        toolVersion = 'fake 1.2.3'
        commandTreeSha256 = '0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef'
    } | ConvertTo-Json | Set-Content `
        -LiteralPath (Join-Path $tempRoot 'src/ModularPipelines.Fake/Generated/Fake.CommandCoverage.json')
    Set-Content `
        -LiteralPath (Join-Path $tempRoot 'src/ModularPipelines.Fake/Options.Generated.cs') `
        -Value 'using Legacy.CommandContext;'
    Set-Content -LiteralPath $changeManifest -Value 'src/ModularPipelines.Fake/Options.Generated.cs'

    Invoke-Git $tempRoot init -b main
    Invoke-Git $tempRoot config user.email test@example.com
    Invoke-Git $tempRoot config user.name 'Generated Options Test'
    Invoke-Git $tempRoot add .
    Invoke-Git $tempRoot commit -m baseline

    $missingProvenanceError = $null
    try {
        & $assertScript `
            -RepositoryRoot $tempRoot `
            -Tool fake `
            -Package ModularPipelines.Fake `
            -NamespacePrefix Fake `
            -CurrentBase HEAD
    }
    catch {
        $missingProvenanceError = $_.Exception.Message
    }
    if ([string]::IsNullOrWhiteSpace($missingProvenanceError) -or
        -not $missingProvenanceError.Contains('no provenance', [StringComparison]::Ordinal)) {
        throw "Missing provenance did not produce a refresh diagnostic: $missingProvenanceError"
    }

    & $writeScript `
        -RepositoryRoot $tempRoot `
        -Tool fake `
        -PackageDirectory src/ModularPipelines.Fake `
        -NamespacePrefix Fake `
        -ChangeManifest $changeManifest
    & $stageScript -RepositoryRoot $tempRoot -ManifestPath $changeManifest
    $stagedPaths = @(git -C $tempRoot diff --cached --name-only)
    if ($stagedPaths -notcontains 'src/ModularPipelines.Fake/Generated/Fake.Generation.json') {
        throw 'Generation provenance was not staged from the change manifest.'
    }

    Invoke-Git $tempRoot add .
    Invoke-Git $tempRoot commit -m generated

    Assert-GeneratedBaselineFreshness -RepositoryRoot $tempRoot

    Set-Content -LiteralPath (Join-Path $tempRoot 'README.md') -Value 'unrelated change'
    Invoke-Git $tempRoot add README.md
    Invoke-Git $tempRoot commit -m unrelated
    & $assertScript `
        -RepositoryRoot $tempRoot `
        -Tool fake `
        -Package ModularPipelines.Fake `
        -NamespacePrefix Fake `
        -CurrentBase HEAD

    Set-Content `
        -LiteralPath (Join-Path $tempRoot 'Directory.Packages.props') `
        -Value '<Project><ItemGroup><PackageVersion Include="AngleSharp" Version="2.0.0" /></ItemGroup></Project>'
    Invoke-Git $tempRoot add Directory.Packages.props
    Invoke-Git $tempRoot commit -m 'update central package version'

    $centralPackageError = $null
    try {
        Assert-GeneratedBaselineFreshness -RepositoryRoot $tempRoot
    }
    catch {
        $centralPackageError = $_.Exception.Message
    }

    if ([string]::IsNullOrWhiteSpace($centralPackageError) -or
        -not $centralPackageError.Contains('are stale', [StringComparison]::Ordinal)) {
        throw "Central package change did not invalidate generated options: $centralPackageError"
    }

    & $writeScript `
        -RepositoryRoot $tempRoot `
        -Tool fake `
        -PackageDirectory src/ModularPipelines.Fake `
        -NamespacePrefix Fake `
        -ChangeManifest $changeManifest
    Invoke-Git $tempRoot add .
    Invoke-Git $tempRoot commit -m 'refresh after central package update'

    Set-Content `
        -LiteralPath (Join-Path $tempRoot 'scripts/Write-GeneratedOptionsProvenance.ps1') `
        -Value '# updated provenance writer'
    Invoke-Git $tempRoot add scripts/Write-GeneratedOptionsProvenance.ps1
    Invoke-Git $tempRoot commit -m 'update provenance writer'

    $provenanceWriterError = $null
    try {
        & $assertScript `
            -RepositoryRoot $tempRoot `
            -Tool fake `
            -Package ModularPipelines.Fake `
            -NamespacePrefix Fake `
            -CurrentBase HEAD
    }
    catch {
        $provenanceWriterError = $_.Exception.Message
    }

    if ([string]::IsNullOrWhiteSpace($provenanceWriterError) -or
        -not $provenanceWriterError.Contains('are stale', [StringComparison]::Ordinal)) {
        throw "Provenance writer change did not invalidate generated options: $provenanceWriterError"
    }

    $coveragePath = Join-Path $tempRoot 'src/ModularPipelines.Fake/Generated/Fake.CommandCoverage.json'
    $validCoverage = Get-Content -LiteralPath $coveragePath -Raw
    $invalidCoverage = $validCoverage | ConvertFrom-Json
    $invalidCoverage.PSObject.Properties.Remove('toolVersion')
    $invalidCoverage | ConvertTo-Json | Set-Content -LiteralPath $coveragePath
    $missingCommandMetadataError = $null
    try {
        & $writeScript `
            -RepositoryRoot $tempRoot `
            -Tool fake `
            -PackageDirectory src/ModularPipelines.Fake `
            -NamespacePrefix Fake `
            -ChangeManifest $changeManifest
    }
    catch {
        $missingCommandMetadataError = $_.Exception.Message
    }
    Set-Content -LiteralPath $coveragePath -Value $validCoverage
    if ([string]::IsNullOrWhiteSpace($missingCommandMetadataError) -or
        -not $missingCommandMetadataError.Contains('incomplete command metadata', [StringComparison]::Ordinal)) {
        throw "Missing coverage metadata was accepted by the provenance writer: $missingCommandMetadataError"
    }

    $invalidCoverage = $validCoverage | ConvertFrom-Json
    $invalidCoverage.commandTreeSha256 = 'not-a-sha256'
    $invalidCoverage | ConvertTo-Json | Set-Content -LiteralPath $coveragePath
    $malformedCommandMetadataError = $null
    try {
        & $writeScript `
            -RepositoryRoot $tempRoot `
            -Tool fake `
            -PackageDirectory src/ModularPipelines.Fake `
            -NamespacePrefix Fake `
            -ChangeManifest $changeManifest
    }
    catch {
        $malformedCommandMetadataError = $_.Exception.Message
    }
    Set-Content -LiteralPath $coveragePath -Value $validCoverage
    if ([string]::IsNullOrWhiteSpace($malformedCommandMetadataError) -or
        -not $malformedCommandMetadataError.Contains('invalid command tree SHA-256', [StringComparison]::Ordinal)) {
        throw "Malformed coverage metadata was accepted by the provenance writer: $malformedCommandMetadataError"
    }

    & $writeScript `
        -RepositoryRoot $tempRoot `
        -Tool fake `
        -PackageDirectory src/ModularPipelines.Fake `
        -NamespacePrefix Fake `
        -ChangeManifest $changeManifest
    Invoke-Git $tempRoot add .
    Invoke-Git $tempRoot commit -m 'refresh after provenance writer update'

    $provenancePath = Join-Path $tempRoot 'src/ModularPipelines.Fake/Generated/Fake.Generation.json'
    $validProvenance = Get-Content -LiteralPath $provenancePath -Raw
    $invalidProvenance = $validProvenance | ConvertFrom-Json
    $invalidProvenance.PSObject.Properties.Remove('commandTreeSha256')
    $invalidProvenance | ConvertTo-Json | Set-Content -LiteralPath $provenancePath
    $invalidProvenanceError = $null
    try {
        & $assertScript `
            -RepositoryRoot $tempRoot `
            -Tool fake `
            -Package ModularPipelines.Fake `
            -NamespacePrefix Fake `
            -CurrentBase HEAD
    }
    catch {
        $invalidProvenanceError = $_.Exception.Message
    }
    Set-Content -LiteralPath $provenancePath -Value $validProvenance
    if ([string]::IsNullOrWhiteSpace($invalidProvenanceError) -or
        -not $invalidProvenanceError.Contains('incomplete command metadata', [StringComparison]::Ordinal)) {
        throw "Incomplete provenance metadata passed freshness validation: $invalidProvenanceError"
    }

    $invalidCoverage = $validCoverage | ConvertFrom-Json
    $invalidCoverage.commandTreeSha256 = ''
    $invalidCoverage | ConvertTo-Json | Set-Content -LiteralPath $coveragePath
    $invalidCoverageError = $null
    try {
        & $assertScript `
            -RepositoryRoot $tempRoot `
            -Tool fake `
            -Package ModularPipelines.Fake `
            -NamespacePrefix Fake `
            -CurrentBase HEAD
    }
    catch {
        $invalidCoverageError = $_.Exception.Message
    }
    Set-Content -LiteralPath $coveragePath -Value $validCoverage
    if ([string]::IsNullOrWhiteSpace($invalidCoverageError) -or
        -not $invalidCoverageError.Contains('incomplete command metadata', [StringComparison]::Ordinal)) {
        throw "Incomplete coverage metadata passed freshness validation: $invalidCoverageError"
    }

    Set-Content `
        -LiteralPath (Join-Path $tempRoot 'tools/ModularPipelines.OptionsGenerator/Emitter.cs') `
        -Value 'using ModularPipelines.Context;'
    Invoke-Git $tempRoot add tools/ModularPipelines.OptionsGenerator/Emitter.cs
    Invoke-Git $tempRoot commit -m 'update generator import'

    $staleError = $null
    try {
        & $assertScript `
            -RepositoryRoot $tempRoot `
            -Tool fake `
            -Package ModularPipelines.Fake `
            -NamespacePrefix Fake `
            -CurrentBase HEAD
    }
    catch {
        $staleError = $_.Exception.Message
    }

    if ([string]::IsNullOrWhiteSpace($staleError) -or
        -not $staleError.Contains('are stale', [StringComparison]::Ordinal) -or
        -not $staleError.Contains('do not rebase old generated commits', [StringComparison]::Ordinal)) {
        throw "Generator import change did not produce the expected stale-snapshot diagnostic: $staleError"
    }

    $manifestPaths = @(Get-Content -LiteralPath $changeManifest)
    if ($manifestPaths -notcontains 'src/ModularPipelines.Fake/Generated/Fake.Generation.json') {
        throw 'Generation provenance was not added to the change manifest.'
    }

    Write-Host 'Generated options provenance tests passed.'
}
finally {
    if (Test-Path -LiteralPath $tempRoot) {
        Remove-Item -LiteralPath $tempRoot -Recurse -Force
    }
    Remove-Item -LiteralPath $changeManifest -Force -ErrorAction SilentlyContinue
}
