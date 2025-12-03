using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "account", "network-profile", "set")]
public record AzBatchAccountNetworkProfileSetOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }
}