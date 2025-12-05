using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "operations", "cancel")]
public record GcloudSqlOperationsCancelOptions(
[property: CliArgument] string Operation
) : GcloudOptions;