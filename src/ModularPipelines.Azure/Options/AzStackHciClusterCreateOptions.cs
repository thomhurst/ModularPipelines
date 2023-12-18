using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci", "cluster", "create")]
public record AzStackHciClusterCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-application-object-id")]
    public string? AadApplicationObjectId { get; set; }

    [CommandSwitch("--aad-client-id")]
    public string? AadClientId { get; set; }

    [CommandSwitch("--aad-service-principal-object-id")]
    public string? AadServicePrincipalObjectId { get; set; }

    [CommandSwitch("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CommandSwitch("--desired-properties")]
    public string? DesiredProperties { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

