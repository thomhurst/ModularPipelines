using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "create", "(aks-preview", "extension)")]
public record AzAksCreateAksPreviewExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CommandSwitch("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CommandSwitch("--aci-subnet-name")]
    public string? AciSubnetName { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CommandSwitch("--api-server-authorized-ip-ranges")]
    public string? ApiServerAuthorizedIpRanges { get; set; }

    [CommandSwitch("--apiserver-subnet-id")]
    public string? ApiserverSubnetId { get; set; }

    [CommandSwitch("--appgw-id")]
    public string? AppgwId { get; set; }

    [CommandSwitch("--appgw-name")]
    public string? AppgwName { get; set; }

    [CommandSwitch("--appgw-subnet-cidr")]
    public string? AppgwSubnetCidr { get; set; }

    [CommandSwitch("--appgw-subnet-id")]
    public string? AppgwSubnetId { get; set; }

    [CommandSwitch("--appgw-watch-namespace")]
    public string? AppgwWatchNamespace { get; set; }

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

    [CommandSwitch("--ca-certs")]
    public string? CaCerts { get; set; }

    [CommandSwitch("--ca-profile")]
    public string? CaProfile { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--cluster-snapshot-id")]
    public string? ClusterSnapshotId { get; set; }

    [CommandSwitch("--crg-id")]
    public string? CrgId { get; set; }

    [CommandSwitch("--data-collection-settings")]
    public string? DataCollectionSettings { get; set; }

    [CommandSwitch("--defender-config")]
    public string? DefenderConfig { get; set; }

    [BooleanCommandSwitch("--disable-disk-driver")]
    public bool? DisableDiskDriver { get; set; }

    [BooleanCommandSwitch("--disable-file-driver")]
    public bool? DisableFileDriver { get; set; }

    [BooleanCommandSwitch("--disable-local-accounts")]
    public bool? DisableLocalAccounts { get; set; }

    [BooleanCommandSwitch("--disable-public-fqdn")]
    public bool? DisablePublicFqdn { get; set; }

    [CommandSwitch("--disable-rbac")]
    public string? DisableRbac { get; set; }

    [BooleanCommandSwitch("--disable-snapshot-controller")]
    public bool? DisableSnapshotController { get; set; }

    [CommandSwitch("--disk-driver-version")]
    public string? DiskDriverVersion { get; set; }

    [CommandSwitch("--dns-name-prefix")]
    public string? DnsNamePrefix { get; set; }

    [CommandSwitch("--dns-service-ip")]
    public string? DnsServiceIp { get; set; }

    [CommandSwitch("--dns-zone-resource-ids")]
    public string? DnsZoneResourceIds { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [BooleanCommandSwitch("--enable-aad")]
    public bool? EnableAad { get; set; }

    [CommandSwitch("--enable-addons")]
    public string? EnableAddons { get; set; }

    [BooleanCommandSwitch("--enable-ahub")]
    public bool? EnableAhub { get; set; }

    [BooleanCommandSwitch("--enable-apiserver-vnet-integration")]
    public bool? EnableApiserverVnetIntegration { get; set; }

    [BooleanCommandSwitch("--enable-app-routing")]
    public bool? EnableAppRouting { get; set; }

    [CommandSwitch("--enable-asm")]
    public string? EnableAsm { get; set; }

    [CommandSwitch("--enable-azure-container-storage")]
    public string? EnableAzureContainerStorage { get; set; }

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

    [BooleanCommandSwitch("--enable-cost-analysis")]
    public bool? EnableCostAnalysis { get; set; }

    [BooleanCommandSwitch("--enable-custom-ca-trust")]
    public bool? EnableCustomCaTrust { get; set; }

    [BooleanCommandSwitch("--enable-defender")]
    public bool? EnableDefender { get; set; }

    [BooleanCommandSwitch("--enable-encryption-at-host")]
    public bool? EnableEncryptionAtHost { get; set; }

    [BooleanCommandSwitch("--enable-fips-image")]
    public bool? EnableFipsImage { get; set; }

    [BooleanCommandSwitch("--enable-image-cleaner")]
    public bool? EnableImageCleaner { get; set; }

    [BooleanCommandSwitch("--enable-image-integrity")]
    public bool? EnableImageIntegrity { get; set; }

    [BooleanCommandSwitch("--enable-keda")]
    public bool? EnableKeda { get; set; }

    [BooleanCommandSwitch("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [BooleanCommandSwitch("--enable-msi-auth-for-monitoring")]
    public bool? EnableMsiAuthForMonitoring { get; set; }

    [CommandSwitch("--enable-network-observability")]
    public string? EnableNetworkObservability { get; set; }

    [BooleanCommandSwitch("--enable-node-public-ip")]
    public bool? EnableNodePublicIp { get; set; }

    [BooleanCommandSwitch("--enable-node-restriction")]
    public bool? EnableNodeRestriction { get; set; }

    [BooleanCommandSwitch("--enable-oidc-issuer")]
    public bool? EnableOidcIssuer { get; set; }

    [BooleanCommandSwitch("--enable-pod-identity")]
    public bool? EnablePodIdentity { get; set; }

    [BooleanCommandSwitch("--enable-pod-identity-with-kubenet")]
    public bool? EnablePodIdentityWithKubenet { get; set; }

    [BooleanCommandSwitch("--enable-private-cluster")]
    public bool? EnablePrivateCluster { get; set; }

    [BooleanCommandSwitch("--enable-secret-rotation")]
    public bool? EnableSecretRotation { get; set; }

    [BooleanCommandSwitch("--enable-sgxquotehelper")]
    public bool? EnableSgxquotehelper { get; set; }

    [BooleanCommandSwitch("--enable-syslog")]
    public bool? EnableSyslog { get; set; }

    [BooleanCommandSwitch("--enable-ultra-ssd")]
    public bool? EnableUltraSsd { get; set; }

    [BooleanCommandSwitch("--enable-vpa")]
    public bool? EnableVpa { get; set; }

    [BooleanCommandSwitch("--enable-windows-gmsa")]
    public bool? EnableWindowsGmsa { get; set; }

    [BooleanCommandSwitch("--enable-windows-recording-rules")]
    public bool? EnableWindowsRecordingRules { get; set; }

    [BooleanCommandSwitch("--enable-workload-identity")]
    public bool? EnableWorkloadIdentity { get; set; }

    [CommandSwitch("--fqdn-subdomain")]
    public string? FqdnSubdomain { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--gmsa-dns-server")]
    public string? GmsaDnsServer { get; set; }

    [CommandSwitch("--gmsa-root-domain-name")]
    public string? GmsaRootDomainName { get; set; }

    [CommandSwitch("--gpu-instance-profile")]
    public string? GpuInstanceProfile { get; set; }

    [CommandSwitch("--grafana-resource-id")]
    public string? GrafanaResourceId { get; set; }

    [CommandSwitch("--guardrails-excluded-ns")]
    public string? GuardrailsExcludedNs { get; set; }

    [CommandSwitch("--guardrails-level")]
    public string? GuardrailsLevel { get; set; }

    [CommandSwitch("--guardrails-version")]
    public string? GuardrailsVersion { get; set; }

    [CommandSwitch("--host-group-id")]
    public string? HostGroupId { get; set; }

    [CommandSwitch("--http-proxy-config")]
    public string? HttpProxyConfig { get; set; }

    [CommandSwitch("--image-cleaner-interval-hours")]
    public string? ImageCleanerIntervalHours { get; set; }

    [CommandSwitch("--ip-families")]
    public string? IpFamilies { get; set; }

    [CommandSwitch("--k8s-support-plan")]
    public string? K8sSupportPlan { get; set; }

    [CommandSwitch("--ksm-metric-annotations-allow-list")]
    public string? KsmMetricAnnotationsAllowList { get; set; }

    [CommandSwitch("--ksm-metric-labels-allow-list")]
    public string? KsmMetricLabelsAllowList { get; set; }

    [CommandSwitch("--kube-proxy-config")]
    public string? KubeProxyConfig { get; set; }

    [CommandSwitch("--kubelet-config")]
    public string? KubeletConfig { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--linux-os-config")]
    public string? LinuxOsConfig { get; set; }

    [CommandSwitch("--load-balancer-backend-pool-type")]
    public string? LoadBalancerBackendPoolType { get; set; }

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

    [CommandSwitch("--load-balancer-sku")]
    public string? LoadBalancerSku { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-count")]
    public int? MaxCount { get; set; }

    [CommandSwitch("--max-pods")]
    public string? MaxPods { get; set; }

    [CommandSwitch("--message-of-the-day")]
    public string? MessageOfTheDay { get; set; }

    [CommandSwitch("--min-count")]
    public int? MinCount { get; set; }

    [CommandSwitch("--nat-gateway-idle-timeout")]
    public string? NatGatewayIdleTimeout { get; set; }

    [CommandSwitch("--nat-gateway-managed-outbound-ip-count")]
    public int? NatGatewayManagedOutboundIpCount { get; set; }

    [CommandSwitch("--network-dataplane")]
    public string? NetworkDataplane { get; set; }

    [CommandSwitch("--network-plugin")]
    public string? NetworkPlugin { get; set; }

    [CommandSwitch("--network-plugin-mode")]
    public string? NetworkPluginMode { get; set; }

    [CommandSwitch("--network-policy")]
    public string? NetworkPolicy { get; set; }

    [BooleanCommandSwitch("--no-ssh-key")]
    public bool? NoSshKey { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--node-os-upgrade-channel")]
    public string? NodeOsUpgradeChannel { get; set; }

    [CommandSwitch("--node-osdisk-diskencryptionset-id")]
    public string? NodeOsdiskDiskencryptionsetId { get; set; }

    [CommandSwitch("--node-osdisk-size")]
    public string? NodeOsdiskSize { get; set; }

    [CommandSwitch("--node-osdisk-type")]
    public string? NodeOsdiskType { get; set; }

    [CommandSwitch("--node-provisioning-mode")]
    public string? NodeProvisioningMode { get; set; }

    [CommandSwitch("--node-public-ip-prefix-id")]
    public string? NodePublicIpPrefixId { get; set; }

    [CommandSwitch("--node-public-ip-tags")]
    public string? NodePublicIpTags { get; set; }

    [CommandSwitch("--node-resource-group")]
    public string? NodeResourceGroup { get; set; }

    [CommandSwitch("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CommandSwitch("--nodepool-allowed-host-ports")]
    public string? NodepoolAllowedHostPorts { get; set; }

    [CommandSwitch("--nodepool-asg-ids")]
    public string? NodepoolAsgIds { get; set; }

    [CommandSwitch("--nodepool-labels")]
    public string? NodepoolLabels { get; set; }

    [CommandSwitch("--nodepool-name")]
    public string? NodepoolName { get; set; }

    [CommandSwitch("--nodepool-tags")]
    public string? NodepoolTags { get; set; }

    [CommandSwitch("--nodepool-taints")]
    public string? NodepoolTaints { get; set; }

    [CommandSwitch("--nrg-lockdown-restriction-level")]
    public string? NrgLockdownRestrictionLevel { get; set; }

    [CommandSwitch("--os-sku")]
    public string? OsSku { get; set; }

    [CommandSwitch("--outbound-type")]
    public string? OutboundType { get; set; }

    [CommandSwitch("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CommandSwitch("--pod-cidrs")]
    public string? PodCidrs { get; set; }

    [CommandSwitch("--pod-subnet-id")]
    public string? PodSubnetId { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [CommandSwitch("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CommandSwitch("--rotation-poll-interval")]
    public string? RotationPollInterval { get; set; }

    [CommandSwitch("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CommandSwitch("--service-cidrs")]
    public string? ServiceCidrs { get; set; }

    [CommandSwitch("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [BooleanCommandSwitch("--skip-subnet-role-assignment")]
    public bool? SkipSubnetRoleAssignment { get; set; }

    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--ssh-key-value")]
    public string? SshKeyValue { get; set; }

    [CommandSwitch("--storage-pool-name")]
    public string? StoragePoolName { get; set; }

    [CommandSwitch("--storage-pool-option")]
    public string? StoragePoolOption { get; set; }

    [CommandSwitch("--storage-pool-size")]
    public string? StoragePoolSize { get; set; }

    [CommandSwitch("--storage-pool-sku")]
    public string? StoragePoolSku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--vm-set-type")]
    public string? VmSetType { get; set; }

    [CommandSwitch("--vnet-subnet-id")]
    public string? VnetSubnetId { get; set; }

    [CommandSwitch("--windows-admin-password")]
    public string? WindowsAdminPassword { get; set; }

    [CommandSwitch("--windows-admin-username")]
    public string? WindowsAdminUsername { get; set; }

    [CommandSwitch("--workload-runtime")]
    public string? WorkloadRuntime { get; set; }

    [CommandSwitch("--workspace-resource-id")]
    public string? WorkspaceResourceId { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}