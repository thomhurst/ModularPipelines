using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "operations", "monitor")]
public record GcloudTransferOperationsMonitorOptions(
[property: CliArgument] string Name
) : GcloudOptions;