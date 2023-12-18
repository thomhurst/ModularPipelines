using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci", "arc-setting", "create")]
public record AzStackHciArcSettingCreateOptions(
[property: CommandSwitch("--arc-setting-name")] string ArcSettingName,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--arc-application-client-id")]
    public string? ArcApplicationClientId { get; set; }

    [CommandSwitch("--arc-application-object-id")]
    public string? ArcApplicationObjectId { get; set; }

    [CommandSwitch("--arc-application-tenant-id")]
    public string? ArcApplicationTenantId { get; set; }

    [CommandSwitch("--arc-instance-rg")]
    public string? ArcInstanceRg { get; set; }

    [CommandSwitch("--arc-service-principal-object-id")]
    public string? ArcServicePrincipalObjectId { get; set; }

    [CommandSwitch("--connectivity-properties")]
    public string? ConnectivityProperties { get; set; }
}