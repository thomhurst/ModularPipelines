using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "operations", "pause")]
public record GcloudTransferOperationsPauseOptions(
[property: CliArgument] string Name
) : GcloudOptions;