using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "operations", "wait")]
public record GcloudContainerBareMetalOperationsWaitOptions(
[property: PositionalArgument] string OperationId,
[property: PositionalArgument] string Location
) : GcloudOptions;