using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "operations", "wait")]
public record GcloudContainerBareMetalOperationsWaitOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;