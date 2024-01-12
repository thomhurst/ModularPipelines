using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "unmanaged", "set-named-ports")]
public record GcloudComputeInstanceGroupsUnmanagedSetNamedPortsOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--named-ports")] string[] NamedPorts
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}