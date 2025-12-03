using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "operations", "describe")]
public record GcloudContainerAwsOperationsDescribeOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;