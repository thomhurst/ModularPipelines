using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "deallocate")]
public record AzVmssDeallocateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--hibernate")]
    public bool? Hibernate { get; set; }

    [CliOption("--instance-ids")]
    public string? InstanceIds { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}