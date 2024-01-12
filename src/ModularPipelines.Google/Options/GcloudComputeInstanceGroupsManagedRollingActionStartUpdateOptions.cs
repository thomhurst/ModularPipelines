using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "rolling-action", "start-update")]
public record GcloudComputeInstanceGroupsManagedRollingActionStartUpdateOptions : GcloudOptions
{
    public GcloudComputeInstanceGroupsManagedRollingActionStartUpdateOptions(
        string name,
        string[] version
    )
    {
        Name = name;
        Version = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Name { get; set; }

    [CommandSwitch("--version")]
    public new string[] Version { get; set; }
}
