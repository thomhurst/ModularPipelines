using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("looker", "operations", "cancel")]
public record GcloudLookerOperationsCancelOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Region
) : GcloudOptions;