using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "os-inventory", "describe")]
public record GcloudComputeInstancesOsInventoryDescribeOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}