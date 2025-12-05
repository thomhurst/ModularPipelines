using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci", "cluster", "create")]
public record AzStackHciClusterCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-application-object-id")]
    public string? AadApplicationObjectId { get; set; }

    [CliOption("--aad-client-id")]
    public string? AadClientId { get; set; }

    [CliOption("--aad-service-principal-object-id")]
    public string? AadServicePrincipalObjectId { get; set; }

    [CliOption("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CliOption("--desired-properties")]
    public string? DesiredProperties { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}