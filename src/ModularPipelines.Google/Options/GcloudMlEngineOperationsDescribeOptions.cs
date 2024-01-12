using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "operations", "describe")]
public record GcloudMlEngineOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}