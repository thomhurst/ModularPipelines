using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "operations", "cancel")]
public record GcloudMlEngineOperationsCancelOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}