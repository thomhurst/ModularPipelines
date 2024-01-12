using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "operations", "cancel")]
public record GcloudTransferOperationsCancelOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;