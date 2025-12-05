using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "operations", "wait")]
public record GcloudSqlOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions
{
    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}