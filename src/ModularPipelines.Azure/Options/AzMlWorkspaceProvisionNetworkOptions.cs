using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "workspace", "provision-network")]
public record AzMlWorkspaceProvisionNetworkOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--include-spark")]
    public bool? IncludeSpark { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}