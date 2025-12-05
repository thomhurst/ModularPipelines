using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "instances", "inject-fault")]
public record GcloudAlloydbInstancesInjectFaultOptions(
[property: CliArgument] string Instance,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--fault-type")] string FaultType,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}