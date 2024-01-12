using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "operations", "cancel")]
public record GcloudStorageOperationsCancelOptions(
[property: PositionalArgument] string OperationName
) : GcloudOptions;