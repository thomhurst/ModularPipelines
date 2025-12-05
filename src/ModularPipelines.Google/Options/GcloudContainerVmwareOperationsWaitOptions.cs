using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "operations", "wait")]
public record GcloudContainerVmwareOperationsWaitOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;