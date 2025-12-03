using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "operations", "describe")]
public record GcloudContainerBareMetalOperationsDescribeOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;