using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "operations", "cancel")]
public record GcloudContainerAzureOperationsCancelOptions(
[property: CliArgument] string OperationId,
[property: CliArgument] string Location
) : GcloudOptions;