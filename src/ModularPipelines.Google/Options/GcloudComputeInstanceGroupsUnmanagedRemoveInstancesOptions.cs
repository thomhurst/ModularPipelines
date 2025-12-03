using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "unmanaged", "remove-instances")]
public record GcloudComputeInstanceGroupsUnmanagedRemoveInstancesOptions(
[property: CliArgument] string Name,
[property: CliOption("--instances")] string[] Instances
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}