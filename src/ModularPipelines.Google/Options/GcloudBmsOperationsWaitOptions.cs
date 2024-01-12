using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "operations", "wait")]
public record GcloudBmsOperationsWaitOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Region
) : GcloudOptions;