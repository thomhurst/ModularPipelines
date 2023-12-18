using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search", "private-endpoint-connection", "update")]
public record AzSearchPrivateEndpointConnectionUpdateOptions(
[property: CommandSwitch("--actions-required")] string ActionsRequired,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--status")] string Status
) : AzOptions;