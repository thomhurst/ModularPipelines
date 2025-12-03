using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databricks", "workspace", "private-endpoint-connection", "create")]
public record AzDatabricksWorkspacePrivateEndpointConnectionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--status")] string Status,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--actions-required")]
    public string? ActionsRequired { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--group-ids")]
    public string? GroupIds { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}