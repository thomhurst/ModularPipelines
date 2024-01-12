using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "operations", "cancel")]
public record GcloudSqlOperationsCancelOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;