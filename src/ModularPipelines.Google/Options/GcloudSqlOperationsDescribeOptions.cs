using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "operations", "describe")]
public record GcloudSqlOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;