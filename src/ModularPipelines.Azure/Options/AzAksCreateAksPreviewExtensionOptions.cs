using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "create", "(aks-preview", "extension)")]
public record AzAksCreateAksPreviewExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CliOption("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CliOption("--aci-subnet-name")]
    public string? AciSubnetName { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliOption("--api-server-authorized-ip-ranges")]
    public string? ApiServerAuthorizedIpRanges { get; set; }

    [CliOption("--apiserver-subnet-id")]
    public string? ApiserverSubnetId { get; set; }

    [CliOption("--appgw-id")]
    public string? AppgwId { get; set; }

    [CliOption("--appgw-name")]
    public string? AppgwName { get; set; }

    [CliOption("--appgw-subnet-cidr")]
    public string? AppgwSubnetCidr { get; set; }

    [CliOption("--appgw-subnet-id")]
    public string? AppgwSubnetId { get; set; }

    [CliOption("--appgw-watch-namespace")]
    public string? AppgwWatchNamespace { get; set; }

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

    [CliOption("--ca-certs")]
    public string? CaCerts { get; set; }

    [CliOption("--ca-profile")]
    public string? CaProfile { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--cluster-snapshot-id")]
    public string? ClusterSnapshotId { get; set; }

    [CliOption("--crg-id")]
    public string? CrgId { get; set; }

    [CliOption("--data-collection-settings")]
    public string? DataCollectionSettings { get; set; }

    [CliOption("--defender-config")]
    public string? DefenderConfig { get; set; }

    [CliFlag("--disable-disk-driver")]
    public bool? DisableDiskDriver { get; set; }

    [CliFlag("--disable-file-driver")]
    public bool? DisableFileDriver { get; set; }

    [CliFlag("--disable-local-accounts")]
    public bool? DisableLocalAccounts { get; set; }

    [CliFlag("--disable-public-fqdn")]
    public bool? DisablePublicFqdn { get; set; }

    [CliFlag("--disable-rbac")]
    public bool? DisableRbac { get; set; }

    [CliFlag("--disable-snapshot-controller")]
    public bool? DisableSnapshotController { get; set; }

    [CliOption("--disk-driver-version")]
    public string? DiskDriverVersion { get; set; }

    [CliOption("--dns-name-prefix")]
    public string? DnsNamePrefix { get; set; }

    [CliOption("--dns-service-ip")]
    public string? DnsServiceIp { get; set; }

    [CliOption("--dns-zone-resource-ids")]
    public string? DnsZoneResourceIds { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliFlag("--enable-aad")]
    public bool? EnableAad { get; set; }

    [CliFlag("--enable-addons")]
    public bool? EnableAddons { get; set; }

    [CliFlag("--enable-ahub")]
    public bool? EnableAhub { get; set; }

    [CliFlag("--enable-apiserver-vnet-integration")]
    public bool? EnableApiserverVnetIntegration { get; set; }

    [CliFlag("--enable-app-routing")]
    public bool? EnableAppRouting { get; set; }

    [CliFlag("--enable-asm")]
    public bool? EnableAsm { get; set; }

    [CliFlag("--enable-azure-container-storage")]
    public bool? EnableAzureContainerStorage { get; set; }

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

    [CliFlag("--enable-cost-analysis")]
    public bool? EnableCostAnalysis { get; set; }

    [CliFlag("--enable-custom-ca-trust")]
    public bool? EnableCustomCaTrust { get; set; }

    [CliFlag("--enable-defender")]
    public bool? EnableDefender { get; set; }

    [CliFlag("--enable-encryption-at-host")]
    public bool? EnableEncryptionAtHost { get; set; }

    [CliFlag("--enable-fips-image")]
    public bool? EnableFipsImage { get; set; }

    [CliFlag("--enable-image-cleaner")]
    public bool? EnableImageCleaner { get; set; }

    [CliFlag("--enable-image-integrity")]
    public bool? EnableImageIntegrity { get; set; }

    [CliFlag("--enable-keda")]
    public bool? EnableKeda { get; set; }

    [CliFlag("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [CliFlag("--enable-msi-auth-for-monitoring")]
    public bool? EnableMsiAuthForMonitoring { get; set; }

    [CliFlag("--enable-network-observability")]
    public bool? EnableNetworkObservability { get; set; }

    [CliFlag("--enable-node-public-ip")]
    public bool? EnableNodePublicIp { get; set; }

    [CliFlag("--enable-node-restriction")]
    public bool? EnableNodeRestriction { get; set; }

    [CliFlag("--enable-oidc-issuer")]
    public bool? EnableOidcIssuer { get; set; }

    [CliFlag("--enable-pod-identity")]
    public bool? EnablePodIdentity { get; set; }

    [CliFlag("--enable-pod-identity-with-kubenet")]
    public bool? EnablePodIdentityWithKubenet { get; set; }

    [CliFlag("--enable-private-cluster")]
    public bool? EnablePrivateCluster { get; set; }

    [CliFlag("--enable-secret-rotation")]
    public bool? EnableSecretRotation { get; set; }

    [CliFlag("--enable-sgxquotehelper")]
    public bool? EnableSgxquotehelper { get; set; }

    [CliFlag("--enable-syslog")]
    public bool? EnableSyslog { get; set; }

    [CliFlag("--enable-ultra-ssd")]
    public bool? EnableUltraSsd { get; set; }

    [CliFlag("--enable-vpa")]
    public bool? EnableVpa { get; set; }

    [CliFlag("--enable-windows-gmsa")]
    public bool? EnableWindowsGmsa { get; set; }

    [CliFlag("--enable-windows-recording-rules")]
    public bool? EnableWindowsRecordingRules { get; set; }

    [CliFlag("--enable-workload-identity")]
    public bool? EnableWorkloadIdentity { get; set; }

    [CliOption("--fqdn-subdomain")]
    public string? FqdnSubdomain { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--gmsa-dns-server")]
    public string? GmsaDnsServer { get; set; }

    [CliOption("--gmsa-root-domain-name")]
    public string? GmsaRootDomainName { get; set; }

    [CliOption("--gpu-instance-profile")]
    public string? GpuInstanceProfile { get; set; }

    [CliOption("--grafana-resource-id")]
    public string? GrafanaResourceId { get; set; }

    [CliOption("--guardrails-excluded-ns")]
    public string? GuardrailsExcludedNs { get; set; }

    [CliOption("--guardrails-level")]
    public string? GuardrailsLevel { get; set; }

    [CliOption("--guardrails-version")]
    public string? GuardrailsVersion { get; set; }

    [CliOption("--host-group-id")]
    public string? HostGroupId { get; set; }

    [CliOption("--http-proxy-config")]
    public string? HttpProxyConfig { get; set; }

    [CliOption("--image-cleaner-interval-hours")]
    public string? ImageCleanerIntervalHours { get; set; }

    [CliOption("--ip-families")]
    public string? IpFamilies { get; set; }

    [CliOption("--k8s-support-plan")]
    public string? K8sSupportPlan { get; set; }

    [CliOption("--ksm-metric-annotations-allow-list")]
    public string? KsmMetricAnnotationsAllowList { get; set; }

    [CliOption("--ksm-metric-labels-allow-list")]
    public string? KsmMetricLabelsAllowList { get; set; }

    [CliOption("--kube-proxy-config")]
    public string? KubeProxyConfig { get; set; }

    [CliOption("--kubelet-config")]
    public string? KubeletConfig { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliOption("--linux-os-config")]
    public string? LinuxOsConfig { get; set; }

    [CliOption("--load-balancer-backend-pool-type")]
    public string? LoadBalancerBackendPoolType { get; set; }

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

    [CliOption("--load-balancer-sku")]
    public string? LoadBalancerSku { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-count")]
    public int? MaxCount { get; set; }

    [CliOption("--max-pods")]
    public string? MaxPods { get; set; }

    [CliOption("--message-of-the-day")]
    public string? MessageOfTheDay { get; set; }

    [CliOption("--min-count")]
    public int? MinCount { get; set; }

    [CliOption("--nat-gateway-idle-timeout")]
    public string? NatGatewayIdleTimeout { get; set; }

    [CliOption("--nat-gateway-managed-outbound-ip-count")]
    public int? NatGatewayManagedOutboundIpCount { get; set; }

    [CliOption("--network-dataplane")]
    public string? NetworkDataplane { get; set; }

    [CliOption("--network-plugin")]
    public string? NetworkPlugin { get; set; }

    [CliOption("--network-plugin-mode")]
    public string? NetworkPluginMode { get; set; }

    [CliOption("--network-policy")]
    public string? NetworkPolicy { get; set; }

    [CliFlag("--no-ssh-key")]
    public bool? NoSshKey { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--node-os-upgrade-channel")]
    public string? NodeOsUpgradeChannel { get; set; }

    [CliOption("--node-osdisk-diskencryptionset-id")]
    public string? NodeOsdiskDiskencryptionsetId { get; set; }

    [CliOption("--node-osdisk-size")]
    public string? NodeOsdiskSize { get; set; }

    [CliOption("--node-osdisk-type")]
    public string? NodeOsdiskType { get; set; }

    [CliOption("--node-provisioning-mode")]
    public string? NodeProvisioningMode { get; set; }

    [CliOption("--node-public-ip-prefix-id")]
    public string? NodePublicIpPrefixId { get; set; }

    [CliOption("--node-public-ip-tags")]
    public string? NodePublicIpTags { get; set; }

    [CliOption("--node-resource-group")]
    public string? NodeResourceGroup { get; set; }

    [CliOption("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CliOption("--nodepool-allowed-host-ports")]
    public string? NodepoolAllowedHostPorts { get; set; }

    [CliOption("--nodepool-asg-ids")]
    public string? NodepoolAsgIds { get; set; }

    [CliOption("--nodepool-labels")]
    public string? NodepoolLabels { get; set; }

    [CliOption("--nodepool-name")]
    public string? NodepoolName { get; set; }

    [CliOption("--nodepool-tags")]
    public string? NodepoolTags { get; set; }

    [CliOption("--nodepool-taints")]
    public string? NodepoolTaints { get; set; }

    [CliOption("--nrg-lockdown-restriction-level")]
    public string? NrgLockdownRestrictionLevel { get; set; }

    [CliOption("--os-sku")]
    public string? OsSku { get; set; }

    [CliOption("--outbound-type")]
    public string? OutboundType { get; set; }

    [CliOption("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CliOption("--pod-cidrs")]
    public string? PodCidrs { get; set; }

    [CliOption("--pod-subnet-id")]
    public string? PodSubnetId { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--rotation-poll-interval")]
    public string? RotationPollInterval { get; set; }

    [CliOption("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CliOption("--service-cidrs")]
    public string? ServiceCidrs { get; set; }

    [CliOption("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CliFlag("--skip-subnet-role-assignment")]
    public bool? SkipSubnetRoleAssignment { get; set; }

    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--ssh-key-value")]
    public string? SshKeyValue { get; set; }

    [CliOption("--storage-pool-name")]
    public string? StoragePoolName { get; set; }

    [CliOption("--storage-pool-option")]
    public string? StoragePoolOption { get; set; }

    [CliOption("--storage-pool-size")]
    public string? StoragePoolSize { get; set; }

    [CliOption("--storage-pool-sku")]
    public string? StoragePoolSku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--vm-set-type")]
    public string? VmSetType { get; set; }

    [CliOption("--vnet-subnet-id")]
    public string? VnetSubnetId { get; set; }

    [CliOption("--windows-admin-password")]
    public string? WindowsAdminPassword { get; set; }

    [CliOption("--windows-admin-username")]
    public string? WindowsAdminUsername { get; set; }

    [CliOption("--workload-runtime")]
    public string? WorkloadRuntime { get; set; }

    [CliOption("--workspace-resource-id")]
    public string? WorkspaceResourceId { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}