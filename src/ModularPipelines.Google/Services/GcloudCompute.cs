using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudCompute
{
    public GcloudCompute(
        GcloudComputeAcceleratorTypes acceleratorTypes,
        GcloudComputeAddresses addresses,
        GcloudComputeBackendBuckets backendBuckets,
        GcloudComputeBackendServices backendServices,
        GcloudComputeCommitments commitments,
        GcloudComputeDiagnose diagnose,
        GcloudComputeDiskTypes diskTypes,
        GcloudComputeDisks disks,
        GcloudComputeExternalVpnGateways externalVpnGateways,
        GcloudComputeFirewallPolicies firewallPolicies,
        GcloudComputeFirewallRules firewallRules,
        GcloudComputeForwardingRules forwardingRules,
        GcloudComputeHealthChecks healthChecks,
        GcloudComputeHttpHealthChecks httpHealthChecks,
        GcloudComputeHttpsHealthChecks httpsHealthChecks,
        GcloudComputeImages images,
        GcloudComputeInstanceGroups instanceGroups,
        GcloudComputeInstanceTemplates instanceTemplates,
        GcloudComputeInstances instances,
        GcloudComputeInterconnects interconnects,
        GcloudComputeMachineImages machineImages,
        GcloudComputeMachineTypes machineTypes,
        GcloudComputeNetworkAttachments networkAttachments,
        GcloudComputeNetworkEdgeSecurityServices networkEdgeSecurityServices,
        GcloudComputeNetworkEndpointGroups networkEndpointGroups,
        GcloudComputeNetworkFirewallPolicies networkFirewallPolicies,
        GcloudComputeNetworks networks,
        GcloudComputeOperations operations,
        GcloudComputeOsConfig osConfig,
        GcloudComputeOsLogin osLogin,
        GcloudComputePacketMirrorings packetMirrorings,
        GcloudComputeProjectInfo projectInfo,
        GcloudComputePublicAdvertisedPrefixes publicAdvertisedPrefixes,
        GcloudComputePublicDelegatedPrefixes publicDelegatedPrefixes,
        GcloudComputeRegions regions,
        GcloudComputeReservations reservations,
        GcloudComputeResourcePolicies resourcePolicies,
        GcloudComputeRouters routers,
        GcloudComputeRoutes routes,
        GcloudComputeSecurityPolicies securityPolicies,
        GcloudComputeServiceAttachments serviceAttachments,
        GcloudComputeSharedVpc sharedVpc,
        GcloudComputeSnapshotSettings snapshotSettings,
        GcloudComputeSnapshots snapshots,
        GcloudComputeSoleTenancy soleTenancy,
        GcloudComputeSslCertificates sslCertificates,
        GcloudComputeSslPolicies sslPolicies,
        GcloudComputeTargetGrpcProxies targetGrpcProxies,
        GcloudComputeTargetHttpProxies targetHttpProxies,
        GcloudComputeTargetHttpsProxies targetHttpsProxies,
        GcloudComputeTargetInstances targetInstances,
        GcloudComputeTargetPools targetPools,
        GcloudComputeTargetSslProxies targetSslProxies,
        GcloudComputeTargetTcpProxies targetTcpProxies,
        GcloudComputeTargetVpnGateways targetVpnGateways,
        GcloudComputeTpus tpus,
        GcloudComputeUrlMaps urlMaps,
        GcloudComputeVpnGateways vpnGateways,
        GcloudComputeVpnTunnels vpnTunnels,
        GcloudComputeZones zones,
        ICommand internalCommand
    )
    {
        AcceleratorTypes = acceleratorTypes;
        Addresses = addresses;
        BackendBuckets = backendBuckets;
        BackendServices = backendServices;
        Commitments = commitments;
        Diagnose = diagnose;
        DiskTypes = diskTypes;
        Disks = disks;
        ExternalVpnGateways = externalVpnGateways;
        FirewallPolicies = firewallPolicies;
        FirewallRules = firewallRules;
        ForwardingRules = forwardingRules;
        HealthChecks = healthChecks;
        HttpHealthChecks = httpHealthChecks;
        HttpsHealthChecks = httpsHealthChecks;
        Images = images;
        InstanceGroups = instanceGroups;
        InstanceTemplates = instanceTemplates;
        Instances = instances;
        Interconnects = interconnects;
        MachineImages = machineImages;
        MachineTypes = machineTypes;
        NetworkAttachments = networkAttachments;
        NetworkEdgeSecurityServices = networkEdgeSecurityServices;
        NetworkEndpointGroups = networkEndpointGroups;
        NetworkFirewallPolicies = networkFirewallPolicies;
        Networks = networks;
        Operations = operations;
        OsConfig = osConfig;
        OsLogin = osLogin;
        PacketMirrorings = packetMirrorings;
        ProjectInfo = projectInfo;
        PublicAdvertisedPrefixes = publicAdvertisedPrefixes;
        PublicDelegatedPrefixes = publicDelegatedPrefixes;
        Regions = regions;
        Reservations = reservations;
        ResourcePolicies = resourcePolicies;
        Routers = routers;
        Routes = routes;
        SecurityPolicies = securityPolicies;
        ServiceAttachments = serviceAttachments;
        SharedVpc = sharedVpc;
        SnapshotSettings = snapshotSettings;
        Snapshots = snapshots;
        SoleTenancy = soleTenancy;
        SslCertificates = sslCertificates;
        SslPolicies = sslPolicies;
        TargetGrpcProxies = targetGrpcProxies;
        TargetHttpProxies = targetHttpProxies;
        TargetHttpsProxies = targetHttpsProxies;
        TargetInstances = targetInstances;
        TargetPools = targetPools;
        TargetSslProxies = targetSslProxies;
        TargetTcpProxies = targetTcpProxies;
        TargetVpnGateways = targetVpnGateways;
        Tpus = tpus;
        UrlMaps = urlMaps;
        VpnGateways = vpnGateways;
        VpnTunnels = vpnTunnels;
        Zones = zones;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeAcceleratorTypes AcceleratorTypes { get; }

    public GcloudComputeAddresses Addresses { get; }

    public GcloudComputeBackendBuckets BackendBuckets { get; }

    public GcloudComputeBackendServices BackendServices { get; }

    public GcloudComputeCommitments Commitments { get; }

    public GcloudComputeDiagnose Diagnose { get; }

    public GcloudComputeDiskTypes DiskTypes { get; }

    public GcloudComputeDisks Disks { get; }

    public GcloudComputeExternalVpnGateways ExternalVpnGateways { get; }

    public GcloudComputeFirewallPolicies FirewallPolicies { get; }

    public GcloudComputeFirewallRules FirewallRules { get; }

    public GcloudComputeForwardingRules ForwardingRules { get; }

    public GcloudComputeHealthChecks HealthChecks { get; }

    public GcloudComputeHttpHealthChecks HttpHealthChecks { get; }

    public GcloudComputeHttpsHealthChecks HttpsHealthChecks { get; }

    public GcloudComputeImages Images { get; }

    public GcloudComputeInstanceGroups InstanceGroups { get; }

    public GcloudComputeInstanceTemplates InstanceTemplates { get; }

    public GcloudComputeInstances Instances { get; }

    public GcloudComputeInterconnects Interconnects { get; }

    public GcloudComputeMachineImages MachineImages { get; }

    public GcloudComputeMachineTypes MachineTypes { get; }

    public GcloudComputeNetworkAttachments NetworkAttachments { get; }

    public GcloudComputeNetworkEdgeSecurityServices NetworkEdgeSecurityServices { get; }

    public GcloudComputeNetworkEndpointGroups NetworkEndpointGroups { get; }

    public GcloudComputeNetworkFirewallPolicies NetworkFirewallPolicies { get; }

    public GcloudComputeNetworks Networks { get; }

    public GcloudComputeOperations Operations { get; }

    public GcloudComputeOsConfig OsConfig { get; }

    public GcloudComputeOsLogin OsLogin { get; }

    public GcloudComputePacketMirrorings PacketMirrorings { get; }

    public GcloudComputeProjectInfo ProjectInfo { get; }

    public GcloudComputePublicAdvertisedPrefixes PublicAdvertisedPrefixes { get; }

    public GcloudComputePublicDelegatedPrefixes PublicDelegatedPrefixes { get; }

    public GcloudComputeRegions Regions { get; }

    public GcloudComputeReservations Reservations { get; }

    public GcloudComputeResourcePolicies ResourcePolicies { get; }

    public GcloudComputeRouters Routers { get; }

    public GcloudComputeRoutes Routes { get; }

    public GcloudComputeSecurityPolicies SecurityPolicies { get; }

    public GcloudComputeServiceAttachments ServiceAttachments { get; }

    public GcloudComputeSharedVpc SharedVpc { get; }

    public GcloudComputeSnapshotSettings SnapshotSettings { get; }

    public GcloudComputeSnapshots Snapshots { get; }

    public GcloudComputeSoleTenancy SoleTenancy { get; }

    public GcloudComputeSslCertificates SslCertificates { get; }

    public GcloudComputeSslPolicies SslPolicies { get; }

    public GcloudComputeTargetGrpcProxies TargetGrpcProxies { get; }

    public GcloudComputeTargetHttpProxies TargetHttpProxies { get; }

    public GcloudComputeTargetHttpsProxies TargetHttpsProxies { get; }

    public GcloudComputeTargetInstances TargetInstances { get; }

    public GcloudComputeTargetPools TargetPools { get; }

    public GcloudComputeTargetSslProxies TargetSslProxies { get; }

    public GcloudComputeTargetTcpProxies TargetTcpProxies { get; }

    public GcloudComputeTargetVpnGateways TargetVpnGateways { get; }

    public GcloudComputeTpus Tpus { get; }

    public GcloudComputeUrlMaps UrlMaps { get; }

    public GcloudComputeVpnGateways VpnGateways { get; }

    public GcloudComputeVpnTunnels VpnTunnels { get; }

    public GcloudComputeZones Zones { get; }

    public async Task<CommandResult> ConfigSsh(GcloudComputeConfigSshOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeConfigSshOptions(), token);
    }

    public async Task<CommandResult> ConnectToSerialPort(GcloudComputeConnectToSerialPortOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyFiles(GcloudComputeCopyFilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetWindowsPassword(GcloudComputeResetWindowsPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scp(GcloudComputeScpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SignUrl(GcloudComputeSignUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Ssh(GcloudComputeSshOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartIapTunnel(GcloudComputeStartIapTunnelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}