using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "namespace", "private-endpoint-connection", "create")]
public record AzServicebusNamespacePrivateEndpointConnectionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}