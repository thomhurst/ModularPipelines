using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "instances", "describe")]
public record GcloudAlloydbInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}