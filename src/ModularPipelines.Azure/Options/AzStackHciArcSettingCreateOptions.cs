using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci", "arc-setting", "create")]
public record AzStackHciArcSettingCreateOptions(
[property: CliOption("--arc-setting-name")] string ArcSettingName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--arc-application-client-id")]
    public string? ArcApplicationClientId { get; set; }

    [CliOption("--arc-application-object-id")]
    public string? ArcApplicationObjectId { get; set; }

    [CliOption("--arc-application-tenant-id")]
    public string? ArcApplicationTenantId { get; set; }

    [CliOption("--arc-instance-rg")]
    public string? ArcInstanceRg { get; set; }

    [CliOption("--arc-service-principal-object-id")]
    public string? ArcServicePrincipalObjectId { get; set; }

    [CliOption("--connectivity-properties")]
    public string? ConnectivityProperties { get; set; }
}