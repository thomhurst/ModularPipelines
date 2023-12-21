using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzNetwork
{
    public AzNetwork(
        AzNetworkAlb alb,
        AzNetworkApplicationGateway applicationGateway,
        AzNetworkAsg asg,
        AzNetworkBastion bastion,
        AzNetworkCrossConnection crossConnection,
        AzNetworkCrossRegionLb crossRegionLb,
        AzNetworkCustomIp customIp,
        AzNetworkDdosProtection ddosProtection,
        AzNetworkDns dns,
        AzNetworkExpressRoute expressRoute,
        AzNetworkFirewall firewall,
        AzNetworkFrontDoor frontDoor,
        AzNetworkIpGroup ipGroup,
        AzNetworkLb lb,
        AzNetworkLocalGateway localGateway,
        AzNetworkManager manager,
        AzNetworkNat nat,
        AzNetworkNic nic,
        AzNetworkNsg nsg,
        AzNetworkP2sVpnGateway p2sVpnGateway,
        AzNetworkPerimeter perimeter,
        AzNetworkPrivateDns privateDns,
        AzNetworkPrivateEndpoint privateEndpoint,
        AzNetworkPrivateEndpointConnection privateEndpointConnection,
        AzNetworkPrivateLinkResource privateLinkResource,
        AzNetworkPrivateLinkService privateLinkService,
        AzNetworkProfile profile,
        AzNetworkPublicIp publicIp,
        AzNetworkRouteFilter routeFilter,
        AzNetworkRouteTable routeTable,
        AzNetworkRouteserver routeserver,
        AzNetworkSecurityPartnerProvider securityPartnerProvider,
        AzNetworkServiceEndpoint serviceEndpoint,
        AzNetworkTrafficManager trafficManager,
        AzNetworkVhub vhub,
        AzNetworkVirtualAppliance virtualAppliance,
        AzNetworkVnet vnet,
        AzNetworkVnetGateway vnetGateway,
        AzNetworkVpnConnection vpnConnection,
        AzNetworkVpnGateway vpnGateway,
        AzNetworkVpnServerConfig vpnServerConfig,
        AzNetworkVpnSite vpnSite,
        AzNetworkVwan vwan,
        AzNetworkWatcher watcher,
        ICommand internalCommand
    )
    {
        Alb = alb;
        ApplicationGateway = applicationGateway;
        Asg = asg;
        Bastion = bastion;
        CrossConnection = crossConnection;
        CrossRegionLb = crossRegionLb;
        CustomIp = customIp;
        DdosProtection = ddosProtection;
        Dns = dns;
        ExpressRoute = expressRoute;
        Firewall = firewall;
        FrontDoor = frontDoor;
        IpGroup = ipGroup;
        Lb = lb;
        LocalGateway = localGateway;
        Manager = manager;
        Nat = nat;
        Nic = nic;
        Nsg = nsg;
        P2sVpnGateway = p2sVpnGateway;
        Perimeter = perimeter;
        PrivateDns = privateDns;
        PrivateEndpoint = privateEndpoint;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        PrivateLinkService = privateLinkService;
        Profile = profile;
        PublicIp = publicIp;
        RouteFilter = routeFilter;
        RouteTable = routeTable;
        Routeserver = routeserver;
        SecurityPartnerProvider = securityPartnerProvider;
        ServiceEndpoint = serviceEndpoint;
        TrafficManager = trafficManager;
        Vhub = vhub;
        VirtualAppliance = virtualAppliance;
        Vnet = vnet;
        VnetGateway = vnetGateway;
        VpnConnection = vpnConnection;
        VpnGateway = vpnGateway;
        VpnServerConfig = vpnServerConfig;
        VpnSite = vpnSite;
        Vwan = vwan;
        Watcher = watcher;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkAlb Alb { get; }

    public AzNetworkApplicationGateway ApplicationGateway { get; }

    public AzNetworkAsg Asg { get; }

    public AzNetworkBastion Bastion { get; }

    public AzNetworkCrossConnection CrossConnection { get; }

    public AzNetworkCrossRegionLb CrossRegionLb { get; }

    public AzNetworkCustomIp CustomIp { get; }

    public AzNetworkDdosProtection DdosProtection { get; }

    public AzNetworkDns Dns { get; }

    public AzNetworkExpressRoute ExpressRoute { get; }

    public AzNetworkFirewall Firewall { get; }

    public AzNetworkFrontDoor FrontDoor { get; }

    public AzNetworkIpGroup IpGroup { get; }

    public AzNetworkLb Lb { get; }

    public AzNetworkLocalGateway LocalGateway { get; }

    public AzNetworkManager Manager { get; }

    public AzNetworkNat Nat { get; }

    public AzNetworkNic Nic { get; }

    public AzNetworkNsg Nsg { get; }

    public AzNetworkP2sVpnGateway P2sVpnGateway { get; }

    public AzNetworkPerimeter Perimeter { get; }

    public AzNetworkPrivateDns PrivateDns { get; }

    public AzNetworkPrivateEndpoint PrivateEndpoint { get; }

    public AzNetworkPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzNetworkPrivateLinkResource PrivateLinkResource { get; }

    public AzNetworkPrivateLinkService PrivateLinkService { get; }

    public AzNetworkProfile Profile { get; }

    public AzNetworkPublicIp PublicIp { get; }

    public AzNetworkRouteFilter RouteFilter { get; }

    public AzNetworkRouteTable RouteTable { get; }

    public AzNetworkRouteserver Routeserver { get; }

    public AzNetworkSecurityPartnerProvider SecurityPartnerProvider { get; }

    public AzNetworkServiceEndpoint ServiceEndpoint { get; }

    public AzNetworkTrafficManager TrafficManager { get; }

    public AzNetworkVhub Vhub { get; }

    public AzNetworkVirtualAppliance VirtualAppliance { get; }

    public AzNetworkVnet Vnet { get; }

    public AzNetworkVnetGateway VnetGateway { get; }

    public AzNetworkVpnConnection VpnConnection { get; }

    public AzNetworkVpnGateway VpnGateway { get; }

    public AzNetworkVpnServerConfig VpnServerConfig { get; }

    public AzNetworkVpnSite VpnSite { get; }

    public AzNetworkVwan Vwan { get; }

    public AzNetworkWatcher Watcher { get; }

    public async Task<CommandResult> ListServiceAliases(AzNetworkListServiceAliasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkListServiceAliasesOptions(), token);
    }

    public async Task<CommandResult> ListServiceTags(AzNetworkListServiceTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsages(AzNetworkListUsagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}