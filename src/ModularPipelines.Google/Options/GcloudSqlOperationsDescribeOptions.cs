using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "operations", "describe")]
public record GcloudSqlOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;