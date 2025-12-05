using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "operations", "describe")]
public record GcloudContainerAzureOperationsDescribeOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;