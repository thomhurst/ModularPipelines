using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "sim", "show")]
public record AzMobileNetworkSimShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sim-group-name")]
    public string? SimGroupName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}