using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("signalr", "replica", "list")]
public record AzSignalrReplicaListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--signalr-name")] string SignalrName
) : AzOptions;