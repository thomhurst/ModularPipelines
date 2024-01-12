using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "operations", "wait")]
public record GcloudContainerAwsOperationsWaitOptions(
[property: PositionalArgument] string OperationId,
[property: PositionalArgument] string Location
) : GcloudOptions;