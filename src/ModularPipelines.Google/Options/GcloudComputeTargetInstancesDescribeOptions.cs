using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-instances", "describe")]
public record GcloudComputeTargetInstancesDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}