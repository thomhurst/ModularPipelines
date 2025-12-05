using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "service-endpoint", "urerm", "create")]
public record AzDevopsServiceEndpointAzurermCreateOptions(
[property: CliOption("--azure-rm-service-principal-id")] string AzureRmServicePrincipalId,
[property: CliOption("--azure-rm-subscription-id")] string AzureRmSubscriptionId,
[property: CliOption("--azure-rm-subscription-name")] string AzureRmSubscriptionName,
[property: CliOption("--azure-rm-tenant-id")] string AzureRmTenantId,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--azure-rm-service-principal-certificate-path")]
    public string? AzureRmServicePrincipalCertificatePath { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}