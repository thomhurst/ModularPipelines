using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "update")]
public record AzAksUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CommandSwitch("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CommandSwitch("--api-server-authorized-ip-ranges")]
    public string? ApiServerAuthorizedIpRanges { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--assign-kubelet-identity")]
    public string? AssignKubeletIdentity { get; set; }

    [BooleanCommandSwitch("--attach-acr")]
    public bool? AttachAcr { get; set; }

    [CommandSwitch("--auto-upgrade-channel")]
    public string? AutoUpgradeChannel { get; set; }

    [CommandSwitch("--azure-keyvault-kms-key-id")]
    public string? AzureKeyvaultKmsKeyId { get; set; }

    [CommandSwitch("--azure-keyvault-kms-key-vault-network-access")]
    public string? AzureKeyvaultKmsKeyVaultNetworkAccess { get; set; }

    [CommandSwitch("--azure-keyvault-kms-key-vault-resource-id")]
    public string? AzureKeyvaultKmsKeyVaultResourceId { get; set; }

    [CommandSwitch("--azure-monitor-workspace-resource-id")]
    public string? AzureMonitorWorkspaceResourceId { get; set; }

    [CommandSwitch("--ca-profile")]
    public string? CaProfile { get; set; }

    [CommandSwitch("--defender-config")]
    public string? DefenderConfig { get; set; }

    [CommandSwitch("--detach-acr")]
    public string? DetachAcr { get; set; }

    [BooleanCommandSwitch("--disable-ahub")]
    public bool? DisableAhub { get; set; }

    [BooleanCommandSwitch("--disable-azure-keyvault-kms")]
    public bool? DisableAzureKeyvaultKms { get; set; }

    [BooleanCommandSwitch("--disable-azure-monitor-metrics")]
    public bool? DisableAzureMonitorMetrics { get; set; }

    [BooleanCommandSwitch("--disable-azure-rbac")]
    public bool? DisableAzureRbac { get; set; }

    [CommandSwitch("--disable-blob-driver")]
    public string? DisableBlobDriver { get; set; }

    [BooleanCommandSwitch("--disable-cluster-autoscaler")]
    public bool? DisableClusterAutoscaler { get; set; }

    [BooleanCommandSwitch("--disable-defender")]
    public bool? DisableDefender { get; set; }

    [BooleanCommandSwitch("--disable-disk-driver")]
    public bool? DisableDiskDriver { get; set; }

    [BooleanCommandSwitch("--disable-file-driver")]
    public bool? DisableFileDriver { get; set; }

    [BooleanCommandSwitch("--disable-force-upgrade")]
    public bool? DisableForceUpgrade { get; set; }

    [BooleanCommandSwitch("--disable-image-cleaner")]
    public bool? DisableImageCleaner { get; set; }

    [BooleanCommandSwitch("--disable-keda")]
    public bool? DisableKeda { get; set; }

    [BooleanCommandSwitch("--disable-local-accounts")]
    public bool? DisableLocalAccounts { get; set; }

    [BooleanCommandSwitch("--disable-public-fqdn")]
    public bool? DisablePublicFqdn { get; set; }

    [BooleanCommandSwitch("--disable-secret-rotation")]
    public bool? DisableSecretRotation { get; set; }

    [BooleanCommandSwitch("--disable-snapshot-controller")]
    public bool? DisableSnapshotController { get; set; }

    [BooleanCommandSwitch("--disable-vpa")]
    public bool? DisableVpa { get; set; }

    [BooleanCommandSwitch("--disable-windows-gmsa")]
    public bool? DisableWindowsGmsa { get; set; }

    [BooleanCommandSwitch("--disable-workload-identity")]
    public bool? DisableWorkloadIdentity { get; set; }

    [BooleanCommandSwitch("--enable-aad")]
    public bool? EnableAad { get; set; }

    [BooleanCommandSwitch("--enable-ahub")]
    public bool? EnableAhub { get; set; }

    [BooleanCommandSwitch("--enable-azure-keyvault-kms")]
    public bool? EnableAzureKeyvaultKms { get; set; }

    [BooleanCommandSwitch("--enable-azure-monitor-metrics")]
    public bool? EnableAzureMonitorMetrics { get; set; }

    [BooleanCommandSwitch("--enable-azure-rbac")]
    public bool? EnableAzureRbac { get; set; }

    [CommandSwitch("--enable-blob-driver")]
    public string? EnableBlobDriver { get; set; }

    [BooleanCommandSwitch("--enable-cluster-autoscaler")]
    public bool? EnableClusterAutoscaler { get; set; }

    [BooleanCommandSwitch("--enable-defender")]
    public bool? EnableDefender { get; set; }

    [BooleanCommandSwitch("--enable-disk-driver")]
    public bool? EnableDiskDriver { get; set; }

    [BooleanCommandSwitch("--enable-file-driver")]
    public bool? EnableFileDriver { get; set; }

    [BooleanCommandSwitch("--enable-force-upgrade")]
    public bool? EnableForceUpgrade { get; set; }

    [BooleanCommandSwitch("--enable-image-cleaner")]
    public bool? EnableImageCleaner { get; set; }

    [BooleanCommandSwitch("--enable-keda")]
    public bool? EnableKeda { get; set; }

    [BooleanCommandSwitch("--enable-local-accounts")]
    public bool? EnableLocalAccounts { get; set; }

    [BooleanCommandSwitch("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [BooleanCommandSwitch("--enable-oidc-issuer")]
    public bool? EnableOidcIssuer { get; set; }

    [BooleanCommandSwitch("--enable-public-fqdn")]
    public bool? EnablePublicFqdn { get; set; }

    [BooleanCommandSwitch("--enable-secret-rotation")]
    public bool? EnableSecretRotation { get; set; }

    [BooleanCommandSwitch("--enable-snapshot-controller")]
    public bool? EnableSnapshotController { get; set; }

    [BooleanCommandSwitch("--enable-vpa")]
    public bool? EnableVpa { get; set; }

    [BooleanCommandSwitch("--enable-windows-gmsa")]
    public bool? EnableWindowsGmsa { get; set; }

    [BooleanCommandSwitch("--enable-windows-recording-rules")]
    public bool? EnableWindowsRecordingRules { get; set; }

    [BooleanCommandSwitch("--enable-workload-identity")]
    public bool? EnableWorkloadIdentity { get; set; }

    [CommandSwitch("--gmsa-dns-server")]
    public string? GmsaDnsServer { get; set; }

    [CommandSwitch("--gmsa-root-domain-name")]
    public string? GmsaRootDomainName { get; set; }

    [CommandSwitch("--grafana-resource-id")]
    public string? GrafanaResourceId { get; set; }

    [CommandSwitch("--http-proxy-config")]
    public string? HttpProxyConfig { get; set; }

    [CommandSwitch("--image-cleaner-interval-hours")]
    public string? ImageCleanerIntervalHours { get; set; }

    [CommandSwitch("--k8s-support-plan")]
    public string? K8sSupportPlan { get; set; }

    [CommandSwitch("--ksm-metric-annotations-allow-list")]
    public string? KsmMetricAnnotationsAllowList { get; set; }

    [CommandSwitch("--ksm-metric-labels-allow-list")]
    public string? KsmMetricLabelsAllowList { get; set; }

    [CommandSwitch("--load-balancer-idle-timeout")]
    public string? LoadBalancerIdleTimeout { get; set; }

    [CommandSwitch("--load-balancer-managed-outbound-ip-count")]
    public int? LoadBalancerManagedOutboundIpCount { get; set; }

    [CommandSwitch("--load-balancer-managed-outbound-ipv6-count")]
    public int? LoadBalancerManagedOutboundIpv6Count { get; set; }

    [CommandSwitch("--load-balancer-outbound-ip-prefixes")]
    public string? LoadBalancerOutboundIpPrefixes { get; set; }

    [CommandSwitch("--load-balancer-outbound-ips")]
    public string? LoadBalancerOutboundIps { get; set; }

    [CommandSwitch("--load-balancer-outbound-ports")]
    public string? LoadBalancerOutboundPorts { get; set; }

    [CommandSwitch("--max-count")]
    public int? MaxCount { get; set; }

    [CommandSwitch("--min-count")]
    public int? MinCount { get; set; }

    [CommandSwitch("--nat-gateway-idle-timeout")]
    public string? NatGatewayIdleTimeout { get; set; }

    [CommandSwitch("--nat-gateway-managed-outbound-ip-count")]
    public int? NatGatewayManagedOutboundIpCount { get; set; }

    [CommandSwitch("--network-dataplane")]
    public string? NetworkDataplane { get; set; }

    [CommandSwitch("--network-plugin-mode")]
    public string? NetworkPluginMode { get; set; }

    [CommandSwitch("--network-policy")]
    public string? NetworkPolicy { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-os-upgrade-channel")]
    public string? NodeOsUpgradeChannel { get; set; }

    [CommandSwitch("--nodepool-labels")]
    public string? NodepoolLabels { get; set; }

    [CommandSwitch("--nodepool-taints")]
    public string? NodepoolTaints { get; set; }

    [CommandSwitch("--outbound-type")]
    public string? OutboundType { get; set; }

    [CommandSwitch("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CommandSwitch("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CommandSwitch("--rotation-poll-interval")]
    public string? RotationPollInterval { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [BooleanCommandSwitch("--update-cluster-autoscaler")]
    public bool? UpdateClusterAutoscaler { get; set; }

    [CommandSwitch("--upgrade-override-until")]
    public string? UpgradeOverrideUntil { get; set; }

    [CommandSwitch("--windows-admin-password")]
    public string? WindowsAdminPassword { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}