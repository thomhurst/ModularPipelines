using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "operations", "describe")]
public record GcloudContainerBareMetalOperationsDescribeOptions(
[property: PositionalArgument] string OperationId,
[property: PositionalArgument] string Location
) : GcloudOptions;