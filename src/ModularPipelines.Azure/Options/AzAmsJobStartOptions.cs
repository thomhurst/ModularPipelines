using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "job", "start")]
public record AzAmsJobStartOptions(
[property: CliOption("--output-assets")] string OutputAssets
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--base-uri")]
    public string? BaseUri { get; set; }

    [CliOption("--correlation-data")]
    public string? CorrelationData { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--files")]
    public string? Files { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--input-asset-name")]
    public string? InputAssetName { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--transform-name")]
    public string? TransformName { get; set; }
}