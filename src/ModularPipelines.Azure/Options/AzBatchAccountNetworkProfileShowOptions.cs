using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "account", "network-profile", "show")]
public record AzBatchAccountNetworkProfileShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}