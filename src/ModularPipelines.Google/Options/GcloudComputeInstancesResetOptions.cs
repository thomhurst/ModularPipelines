using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "reset")]
public record GcloudComputeInstancesResetOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}