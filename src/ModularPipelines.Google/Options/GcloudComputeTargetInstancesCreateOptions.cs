using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-instances", "create")]
public record GcloudComputeTargetInstancesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--instance-zone")]
    public string? InstanceZone { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}