using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "operations", "describe")]
public record GcloudContainerAttachedOperationsDescribeOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;