using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "restart")]
public record AzVmssRestartOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--instance-ids")]
    public string? InstanceIds { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}