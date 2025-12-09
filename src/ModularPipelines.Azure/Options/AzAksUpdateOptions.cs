using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "update")]
public record AzAksUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CliOption("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliOption("--api-server-authorized-ip-ranges")]
    public string? ApiServerAuthorizedIpRanges { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--assign-kubelet-identity")]
    public string? AssignKubeletIdentity { get; set; }

    [CliFlag("--attach-acr")]
    public bool? AttachAcr { get; set; }

    [CliOption("--auto-upgrade-channel")]
    public string? AutoUpgradeChannel { get; set; }

    [CliOption("--azure-keyvault-kms-key-id")]
    public string? AzureKeyvaultKmsKeyId { get; set; }

    [CliOption("--azure-keyvault-kms-key-vault-network-access")]
    public string? AzureKeyvaultKmsKeyVaultNetworkAccess { get; set; }

    [CliOption("--azure-keyvault-kms-key-vault-resource-id")]
    public string? AzureKeyvaultKmsKeyVaultResourceId { get; set; }

    [CliOption("--azure-monitor-workspace-resource-id")]
    public string? AzureMonitorWorkspaceResourceId { get; set; }

    [CliOption("--ca-profile")]
    public string? CaProfile { get; set; }

    [CliOption("--defender-config")]
    public string? DefenderConfig { get; set; }

    [CliOption("--detach-acr")]
    public string? DetachAcr { get; set; }

    [CliFlag("--disable-ahub")]
    public bool? DisableAhub { get; set; }

    [CliFlag("--disable-azure-keyvault-kms")]
    public bool? DisableAzureKeyvaultKms { get; set; }

    [CliFlag("--disable-azure-monitor-metrics")]
    public bool? DisableAzureMonitorMetrics { get; set; }

    [CliFlag("--disable-azure-rbac")]
    public bool? DisableAzureRbac { get; set; }

    [CliFlag("--disable-blob-driver")]
    public bool? DisableBlobDriver { get; set; }

    [CliFlag("--disable-cluster-autoscaler")]
    public bool? DisableClusterAutoscaler { get; set; }

    [CliFlag("--disable-defender")]
    public bool? DisableDefender { get; set; }

    [CliFlag("--disable-disk-driver")]
    public bool? DisableDiskDriver { get; set; }

    [CliFlag("--disable-file-driver")]
    public bool? DisableFileDriver { get; set; }

    [CliFlag("--disable-force-upgrade")]
    public bool? DisableForceUpgrade { get; set; }

    [CliFlag("--disable-image-cleaner")]
    public bool? DisableImageCleaner { get; set; }

    [CliFlag("--disable-keda")]
    public bool? DisableKeda { get; set; }

    [CliFlag("--disable-local-accounts")]
    public bool? DisableLocalAccounts { get; set; }

    [CliFlag("--disable-public-fqdn")]
    public bool? DisablePublicFqdn { get; set; }

    [CliFlag("--disable-secret-rotation")]
    public bool? DisableSecretRotation { get; set; }

    [CliFlag("--disable-snapshot-controller")]
    public bool? DisableSnapshotController { get; set; }

    [CliFlag("--disable-vpa")]
    public bool? DisableVpa { get; set; }

    [CliFlag("--disable-windows-gmsa")]
    public bool? DisableWindowsGmsa { get; set; }

    [CliFlag("--disable-workload-identity")]
    public bool? DisableWorkloadIdentity { get; set; }

    [CliFlag("--enable-aad")]
    public bool? EnableAad { get; set; }

    [CliFlag("--enable-ahub")]
    public bool? EnableAhub { get; set; }

    [CliFlag("--enable-azure-keyvault-kms")]
    public bool? EnableAzureKeyvaultKms { get; set; }

    [CliFlag("--enable-azure-monitor-metrics")]
    public bool? EnableAzureMonitorMetrics { get; set; }

    [CliFlag("--enable-azure-rbac")]
    public bool? EnableAzureRbac { get; set; }

    [CliFlag("--enable-blob-driver")]
    public bool? EnableBlobDriver { get; set; }

    [CliFlag("--enable-cluster-autoscaler")]
    public bool? EnableClusterAutoscaler { get; set; }

    [CliFlag("--enable-defender")]
    public bool? EnableDefender { get; set; }

    [CliFlag("--enable-disk-driver")]
    public bool? EnableDiskDriver { get; set; }

    [CliFlag("--enable-file-driver")]
    public bool? EnableFileDriver { get; set; }

    [CliFlag("--enable-force-upgrade")]
    public bool? EnableForceUpgrade { get; set; }

    [CliFlag("--enable-image-cleaner")]
    public bool? EnableImageCleaner { get; set; }

    [CliFlag("--enable-keda")]
    public bool? EnableKeda { get; set; }

    [CliFlag("--enable-local-accounts")]
    public bool? EnableLocalAccounts { get; set; }

    [CliFlag("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [CliFlag("--enable-oidc-issuer")]
    public bool? EnableOidcIssuer { get; set; }

    [CliFlag("--enable-public-fqdn")]
    public bool? EnablePublicFqdn { get; set; }

    [CliFlag("--enable-secret-rotation")]
    public bool? EnableSecretRotation { get; set; }

    [CliFlag("--enable-snapshot-controller")]
    public bool? EnableSnapshotController { get; set; }

    [CliFlag("--enable-vpa")]
    public bool? EnableVpa { get; set; }

    [CliFlag("--enable-windows-gmsa")]
    public bool? EnableWindowsGmsa { get; set; }

    [CliFlag("--enable-windows-recording-rules")]
    public bool? EnableWindowsRecordingRules { get; set; }

    [CliFlag("--enable-workload-identity")]
    public bool? EnableWorkloadIdentity { get; set; }

    [CliOption("--gmsa-dns-server")]
    public string? GmsaDnsServer { get; set; }

    [CliOption("--gmsa-root-domain-name")]
    public string? GmsaRootDomainName { get; set; }

    [CliOption("--grafana-resource-id")]
    public string? GrafanaResourceId { get; set; }

    [CliOption("--http-proxy-config")]
    public string? HttpProxyConfig { get; set; }

    [CliOption("--image-cleaner-interval-hours")]
    public string? ImageCleanerIntervalHours { get; set; }

    [CliOption("--k8s-support-plan")]
    public string? K8sSupportPlan { get; set; }

    [CliOption("--ksm-metric-annotations-allow-list")]
    public string? KsmMetricAnnotationsAllowList { get; set; }

    [CliOption("--ksm-metric-labels-allow-list")]
    public string? KsmMetricLabelsAllowList { get; set; }

    [CliOption("--load-balancer-idle-timeout")]
    public string? LoadBalancerIdleTimeout { get; set; }

    [CliOption("--load-balancer-managed-outbound-ip-count")]
    public int? LoadBalancerManagedOutboundIpCount { get; set; }

    [CliOption("--load-balancer-managed-outbound-ipv6-count")]
    public int? LoadBalancerManagedOutboundIpv6Count { get; set; }

    [CliOption("--load-balancer-outbound-ip-prefixes")]
    public string? LoadBalancerOutboundIpPrefixes { get; set; }

    [CliOption("--load-balancer-outbound-ips")]
    public string? LoadBalancerOutboundIps { get; set; }

    [CliOption("--load-balancer-outbound-ports")]
    public string? LoadBalancerOutboundPorts { get; set; }

    [CliOption("--max-count")]
    public int? MaxCount { get; set; }

    [CliOption("--min-count")]
    public int? MinCount { get; set; }

    [CliOption("--nat-gateway-idle-timeout")]
    public string? NatGatewayIdleTimeout { get; set; }

    [CliOption("--nat-gateway-managed-outbound-ip-count")]
    public int? NatGatewayManagedOutboundIpCount { get; set; }

    [CliOption("--network-dataplane")]
    public string? NetworkDataplane { get; set; }

    [CliOption("--network-plugin-mode")]
    public string? NetworkPluginMode { get; set; }

    [CliOption("--network-policy")]
    public string? NetworkPolicy { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-os-upgrade-channel")]
    public string? NodeOsUpgradeChannel { get; set; }

    [CliOption("--nodepool-labels")]
    public string? NodepoolLabels { get; set; }

    [CliOption("--nodepool-taints")]
    public string? NodepoolTaints { get; set; }

    [CliOption("--outbound-type")]
    public string? OutboundType { get; set; }

    [CliOption("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--rotation-poll-interval")]
    public string? RotationPollInterval { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliFlag("--update-cluster-autoscaler")]
    public bool? UpdateClusterAutoscaler { get; set; }

    [CliOption("--upgrade-override-until")]
    public string? UpgradeOverrideUntil { get; set; }

    [CliOption("--windows-admin-password")]
    public string? WindowsAdminPassword { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}