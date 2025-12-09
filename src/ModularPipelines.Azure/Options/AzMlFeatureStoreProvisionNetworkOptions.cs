using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "feature-store", "provision-network")]
public record AzMlFeatureStoreProvisionNetworkOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--include-spark")]
    public bool? IncludeSpark { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}