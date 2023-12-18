using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "job", "start")]
public record AzAmsJobStartOptions(
[property: CommandSwitch("--output-assets")] string OutputAssets
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--base-uri")]
    public string? BaseUri { get; set; }

    [CommandSwitch("--correlation-data")]
    public string? CorrelationData { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--files")]
    public string? Files { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--input-asset-name")]
    public string? InputAssetName { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--transform-name")]
    public string? TransformName { get; set; }
}