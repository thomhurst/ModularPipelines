using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "operations", "cancel")]
public record GcloudContainerAwsOperationsCancelOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;