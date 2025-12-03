using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("search", "private-endpoint-connection", "update")]
public record AzSearchPrivateEndpointConnectionUpdateOptions(
[property: CliOption("--actions-required")] string ActionsRequired,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--status")] string Status
) : AzOptions;