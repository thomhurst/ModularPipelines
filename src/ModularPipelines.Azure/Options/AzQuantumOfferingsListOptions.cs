using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("quantum", "offerings", "list")]
public record AzQuantumOfferingsListOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliFlag("--autoadd-only")]
    public bool? AutoaddOnly { get; set; }
}