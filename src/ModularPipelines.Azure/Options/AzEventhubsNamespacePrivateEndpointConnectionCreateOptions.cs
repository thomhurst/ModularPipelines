using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "private-endpoint-connection", "create")]
public record AzEventhubsNamespacePrivateEndpointConnectionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}