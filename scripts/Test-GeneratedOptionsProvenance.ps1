$ErrorActionPreference = 'Stop'

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

$repositoryRoot = Split-Path $PSScriptRoot -Parent
$tempRoot = Join-Path ([IO.Path]::GetTempPath()) "generated-options-provenance-$([Guid]::NewGuid().ToString('N'))"
$writeScript = Join-Path $PSScriptRoot 'Write-GeneratedOptionsProvenance.ps1'
$assertScript = Join-Path $PSScriptRoot 'Assert-GeneratedOptionsFreshness.ps1'
$provenanceFunctions = Join-Path $PSScriptRoot 'GeneratedOptionsProvenance.ps1'
$stageScript = Join-Path `
    $repositoryRoot `
    'tools/ModularPipelines.OptionsGenerator/scripts/Stage-GeneratedChanges.ps1'
$changeManifest = "$tempRoot.manifest"

try {
    . $provenanceFunctions
    New-Item -ItemType Directory -Path $tempRoot | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tempRoot '.github/workflows') | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tempRoot 'tools/ModularPipelines.OptionsGenerator') | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tempRoot 'src/ModularPipelines.Fake/Generated') | Out-Null

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
        commandTreeSha256 = 'command-fingerprint'
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

    & $assertScript `
        -RepositoryRoot $tempRoot `
        -Tool fake `
        -Package ModularPipelines.Fake `
        -NamespacePrefix Fake `
        -CurrentBase HEAD

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
