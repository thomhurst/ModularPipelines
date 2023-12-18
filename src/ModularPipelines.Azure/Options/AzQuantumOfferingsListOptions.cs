using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "offerings", "list")]
public record AzQuantumOfferingsListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [BooleanCommandSwitch("--autoadd-only")]
    public bool? AutoaddOnly { get; set; }
}

