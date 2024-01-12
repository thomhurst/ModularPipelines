using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "operations", "wait")]
public record GcloudEndpointsOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;