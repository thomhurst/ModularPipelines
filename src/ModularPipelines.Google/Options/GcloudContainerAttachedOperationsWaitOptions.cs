using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "operations", "wait")]
public record GcloudContainerAttachedOperationsWaitOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;