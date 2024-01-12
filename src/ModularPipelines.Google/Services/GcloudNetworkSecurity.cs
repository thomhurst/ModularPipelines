using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudNetworkSecurity
{
    public GcloudNetworkSecurity(
        GcloudNetworkSecurityAddressGroups addressGroups,
        GcloudNetworkSecurityAuthorizationPolicies authorizationPolicies,
        GcloudNetworkSecurityClientTlsPolicies clientTlsPolicies,
        GcloudNetworkSecurityGatewaySecurityPolicies gatewaySecurityPolicies,
        GcloudNetworkSecurityOrgAddressGroups orgAddressGroups,
        GcloudNetworkSecurityServerTlsPolicies serverTlsPolicies,
        GcloudNetworkSecurityTlsInspectionPolicies tlsInspectionPolicies,
        GcloudNetworkSecurityUrlLists urlLists
    )
    {
        AddressGroups = addressGroups;
        AuthorizationPolicies = authorizationPolicies;
        ClientTlsPolicies = clientTlsPolicies;
        GatewaySecurityPolicies = gatewaySecurityPolicies;
        OrgAddressGroups = orgAddressGroups;
        ServerTlsPolicies = serverTlsPolicies;
        TlsInspectionPolicies = tlsInspectionPolicies;
        UrlLists = urlLists;
    }

    public GcloudNetworkSecurityAddressGroups AddressGroups { get; }

    public GcloudNetworkSecurityAuthorizationPolicies AuthorizationPolicies { get; }

    public GcloudNetworkSecurityClientTlsPolicies ClientTlsPolicies { get; }

    public GcloudNetworkSecurityGatewaySecurityPolicies GatewaySecurityPolicies { get; }

    public GcloudNetworkSecurityOrgAddressGroups OrgAddressGroups { get; }

    public GcloudNetworkSecurityServerTlsPolicies ServerTlsPolicies { get; }

    public GcloudNetworkSecurityTlsInspectionPolicies TlsInspectionPolicies { get; }

    public GcloudNetworkSecurityUrlLists UrlLists { get; }
}