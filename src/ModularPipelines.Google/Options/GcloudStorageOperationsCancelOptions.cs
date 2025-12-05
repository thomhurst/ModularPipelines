using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "operations", "cancel")]
public record GcloudStorageOperationsCancelOptions(
[property: CliArgument] string OperationName
) : GcloudOptions;