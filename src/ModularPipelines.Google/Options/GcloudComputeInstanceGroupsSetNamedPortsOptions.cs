using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "set-named-ports")]
public record GcloudComputeInstanceGroupsSetNamedPortsOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--named-ports")] string[] NamedPorts
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}