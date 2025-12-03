using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "instances", "create-secondary")]
public record GcloudAlloydbInstancesCreateSecondaryOptions(
[property: CliArgument] string Instance,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--availability-type")]
    public string? AvailabilityType { get; set; }
}