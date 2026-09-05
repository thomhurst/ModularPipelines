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

    Write-Output 'OK public API change summary reports cross-tool assembly impact.'
}
finally {
    if (Test-Path -LiteralPath $temporaryDirectory) {
        Remove-Item -LiteralPath $temporaryDirectory -Recurse -Force
    }
}
