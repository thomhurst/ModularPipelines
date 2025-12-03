using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "start")]
public record AzVmssStartOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--instance-ids")]
    public string? InstanceIds { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}