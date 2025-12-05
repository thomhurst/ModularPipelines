using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "operations", "cancel")]
public record GcloudTransferOperationsCancelOptions(
[property: CliArgument] string Name
) : GcloudOptions;