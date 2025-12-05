using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "describe")]
public record GcloudComputeInstancesDescribeOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}