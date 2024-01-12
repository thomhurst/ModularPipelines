using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "operations", "wait")]
public record GcloudSqlOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions
{
    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}