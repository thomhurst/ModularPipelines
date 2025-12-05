using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "data-connector", "disconnect")]
public record AzSentinelDataConnectorDisconnectOptions(
[property: CliOption("--data-connector-id")] string DataConnectorId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;