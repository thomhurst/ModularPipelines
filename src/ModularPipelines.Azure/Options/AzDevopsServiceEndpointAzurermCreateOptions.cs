using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "service-endpoint", "urerm", "create")]
public record AzDevopsServiceEndpointAzurermCreateOptions(
[property: CommandSwitch("--azure-rm-service-principal-id")] string AzureRmServicePrincipalId,
[property: CommandSwitch("--azure-rm-subscription-id")] string AzureRmSubscriptionId,
[property: CommandSwitch("--azure-rm-subscription-name")] string AzureRmSubscriptionName,
[property: CommandSwitch("--azure-rm-tenant-id")] string AzureRmTenantId,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--azure-rm-service-principal-certificate-path")]
    public string? AzureRmServicePrincipalCertificatePath { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}