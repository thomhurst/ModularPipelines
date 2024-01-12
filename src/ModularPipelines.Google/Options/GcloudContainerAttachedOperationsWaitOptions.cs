using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "attached", "operations", "wait")]
public record GcloudContainerAttachedOperationsWaitOptions(
[property: PositionalArgument] string OperationId,
[property: PositionalArgument] string Location
) : GcloudOptions;