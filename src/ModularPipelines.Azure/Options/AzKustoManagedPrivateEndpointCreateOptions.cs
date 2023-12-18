using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "managed-private-endpoint", "create")]
public record AzKustoManagedPrivateEndpointCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--managed-private-endpoint-name")] string ManagedPrivateEndpointName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-link")]
    public string? PrivateLink { get; set; }

    [CommandSwitch("--private-link-region")]
    public string? PrivateLinkRegion { get; set; }

    [CommandSwitch("--request-message")]
    public string? RequestMessage { get; set; }
}