using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "data-connector", "disconnect")]
public record AzSentinelDataConnectorDisconnectOptions(
[property: CommandSwitch("--data-connector-id")] string DataConnectorId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;