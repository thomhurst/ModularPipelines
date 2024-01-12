using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "operations", "cancel")]
public record GcloudContainerAzureOperationsCancelOptions(
[property: PositionalArgument] string OperationId,
[property: PositionalArgument] string Location
) : GcloudOptions;