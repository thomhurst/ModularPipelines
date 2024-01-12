using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "set-name")]
public record GcloudComputeInstancesSetNameOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--new-name")] string NewName
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}