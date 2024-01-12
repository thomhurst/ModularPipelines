using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "operations", "wait")]
public record GcloudAppOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;