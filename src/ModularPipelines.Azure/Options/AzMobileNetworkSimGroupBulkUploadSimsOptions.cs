using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "sim", "group", "bulk-upload-sims")]
public record AzMobileNetworkSimGroupBulkUploadSimsOptions(
[property: CommandSwitch("--sims")] string Sims
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sim-group-name")]
    public string? SimGroupName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}