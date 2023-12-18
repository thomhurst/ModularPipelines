using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "offerings", "list")]
public record AzQuantumOfferingsListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [BooleanCommandSwitch("--autoadd-only")]
    public bool? AutoaddOnly { get; set; }
}