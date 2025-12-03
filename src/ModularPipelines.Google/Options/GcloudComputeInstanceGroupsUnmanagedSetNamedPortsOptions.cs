using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "unmanaged", "set-named-ports")]
public record GcloudComputeInstanceGroupsUnmanagedSetNamedPortsOptions(
[property: CliArgument] string Name,
[property: CliOption("--named-ports")] string[] NamedPorts
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}