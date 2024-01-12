using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "machine-types", "describe")]
public record GcloudComputeMachineTypesDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}