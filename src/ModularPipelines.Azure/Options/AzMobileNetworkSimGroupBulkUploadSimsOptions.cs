using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "sim", "group", "bulk-upload-sims")]
public record AzMobileNetworkSimGroupBulkUploadSimsOptions(
[property: CliOption("--sims")] string Sims
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sim-group-name")]
    public string? SimGroupName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}