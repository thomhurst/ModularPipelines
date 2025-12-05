using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "operations", "describe")]
public record GcloudContainerVmwareOperationsDescribeOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;