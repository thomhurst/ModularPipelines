$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$temporaryDirectory = Join-Path ([IO.Path]::GetTempPath()) "public-api-summary-$([guid]::NewGuid().ToString('N'))"
$packageDirectory = Join-Path $temporaryDirectory 'src/ModularPipelines.Kubernetes'
$generatedDirectory = Join-Path $packageDirectory 'Generated'
[IO.Directory]::CreateDirectory($generatedDirectory) | Out-Null

try {
    $originalShipped = Join-Path $temporaryDirectory 'PublicAPI.Shipped.original.txt'
    $originalUnshipped = Join-Path $temporaryDirectory 'PublicAPI.Unshipped.original.txt'
    $currentShipped = Join-Path $packageDirectory 'PublicAPI.Shipped.txt'
    $currentUnshipped = Join-Path $packageDirectory 'PublicAPI.Unshipped.txt'
    $summaryPath = Join-Path $temporaryDirectory 'summary.md'

    [IO.File]::WriteAllLines($originalShipped, @(
        '#nullable enable',
        'ModularPipelines.Kubernetes.Options.KubernetesApplyOptions.DryRun.get -> string?',
        'ModularPipelines.Kubernetes.Services.IKubernetesApply.ApplyAsync(ModularPipelines.Kubernetes.Options.KubernetesApplyOptions? options = null) -> System.Threading.Tasks.Task!'
    ))
    [IO.File]::WriteAllLines($originalUnshipped, @())
    # Removed members keep their shipped entry; the *REMOVED* marker retires them.
    [IO.File]::WriteAllLines($currentShipped, @(
        '#nullable enable',
        'ModularPipelines.Kubernetes.Options.KubernetesApplyOptions.DryRun.get -> string?',
        'ModularPipelines.Kubernetes.Services.IKubernetesApply.ApplyAsync(ModularPipelines.Kubernetes.Options.KubernetesApplyOptions? options = null) -> System.Threading.Tasks.Task!'
    ))
    [IO.File]::WriteAllLines($currentUnshipped, @(
        '*REMOVED*ModularPipelines.Kubernetes.Options.KubernetesApplyOptions.DryRun.get -> string?',
        '*REMOVED*ModularPipelines.Kubernetes.Services.IKubernetesApply.ApplyAsync(ModularPipelines.Kubernetes.Options.KubernetesApplyOptions? options = null) -> System.Threading.Tasks.Task!',
        'ModularPipelines.Kubernetes.Options.KubernetesApplyOptions.DryRun.get -> ModularPipelines.Kubernetes.Enums.KubernetesApplyDryRun?',
        'ModularPipelines.Kubernetes.Options.KustomizeBuildOptions.EnableAlphaPlugins.get -> bool?'
    ))
    [IO.File]::WriteAllText((Join-Path $generatedDirectory 'Kubernetes.CommandCoverage.json'), '{"toolName":"kubectl"}')
    [IO.File]::WriteAllText((Join-Path $generatedDirectory 'Kustomize.CommandCoverage.json'), '{"toolName":"kustomize"}')

    & (Join-Path $PSScriptRoot 'Write-PublicApiChangeSummary.ps1') `
        -OriginalShippedPath $originalShipped `
        -OriginalUnshippedPath $originalUnshipped `
        -CurrentShippedPath $currentShipped `
        -CurrentUnshippedPath $currentUnshipped `
        -PackageDirectory $packageDirectory `
        -OutputPath $summaryPath

    $summary = Get-Content -LiteralPath $summaryPath -Raw
    foreach ($expected in @(
        'Affected API families: `Kubernetes (kubectl)`, `Kustomize`.',
        'Breaking changes are present.',
        '- Added APIs: 2',
        '- Removed or changed APIs: 2',
        'Members with matching names but changed signatures: 1',
        'KubernetesApplyOptions.DryRun.get -> string?',
        'KustomizeBuildOptions.EnableAlphaPlugins.get -> bool?')) {
        if (-not $summary.Contains($expected, [StringComparison]::Ordinal)) {
            throw "Summary did not contain expected text: $expected`n$summary"
        }
    }

    # The summary must compare marker payloads exactly, as the validator does.
    # Trailing whitespace cannot turn an invalid marker into a valid removal.
    [IO.File]::WriteAllLines($originalShipped, @('Api.Kept'))
    [IO.File]::WriteAllLines($currentShipped, @('Api.Kept'))
    [IO.File]::WriteAllLines($currentUnshipped, @('*REMOVED*Api.Kept '))
    & (Join-Path $PSScriptRoot 'Write-PublicApiChangeSummary.ps1') `
        -OriginalShippedPath $originalShipped `
        -OriginalUnshippedPath $originalUnshipped `
        -CurrentShippedPath $currentShipped `
        -CurrentUnshippedPath $currentUnshipped `
        -PackageDirectory $packageDirectory `
        -OutputPath $summaryPath
    if (-not (Get-Content -LiteralPath $summaryPath -Raw).Contains('No active public API changes')) {
        throw 'The summary trimmed a removal payload before comparing it.'
    }

    foreach ($indentedEntry in @(' *REMOVED*Api.Kept', ' #nullable enable')) {
        [IO.File]::WriteAllLines($currentUnshipped, @($indentedEntry))
        & (Join-Path $PSScriptRoot 'Write-PublicApiChangeSummary.ps1') `
            -OriginalShippedPath $originalShipped `
            -OriginalUnshippedPath $originalUnshipped `
            -CurrentShippedPath $currentShipped `
            -CurrentUnshippedPath $currentUnshipped `
            -PackageDirectory $packageDirectory `
            -OutputPath $summaryPath
        $summary = Get-Content -LiteralPath $summaryPath -Raw
        if (-not $summary.Contains('- Added APIs: 1') -or -not $summary.Contains('- Removed or changed APIs: 0')) {
            throw "The summary normalized the API line '$indentedEntry': $summary"
        }
    }

    Write-Output 'OK public API change summary reports cross-tool assembly impact and preserves exact markers.'
}
finally {
    if (Test-Path -LiteralPath $temporaryDirectory) {
        Remove-Item -LiteralPath $temporaryDirectory -Recurse -Force
    }
}
