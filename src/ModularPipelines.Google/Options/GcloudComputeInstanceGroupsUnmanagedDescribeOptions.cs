using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "unmanaged", "describe")]
public record GcloudComputeInstanceGroupsUnmanagedDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}