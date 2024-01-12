using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "operations", "wait")]
public record GcloudMlEngineOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}