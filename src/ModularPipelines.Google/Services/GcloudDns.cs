using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDns
{
    public GcloudDns(
        GcloudDnsDnsKeys dnsKeys,
        GcloudDnsManagedZones managedZones,
        GcloudDnsOperations operations,
        GcloudDnsPolicies policies,
        GcloudDnsProjectInfo projectInfo,
        GcloudDnsRecordSets recordSets,
        GcloudDnsResponsePolicies responsePolicies
    )
    {
        DnsKeys = dnsKeys;
        ManagedZones = managedZones;
        Operations = operations;
        Policies = policies;
        ProjectInfo = projectInfo;
        RecordSets = recordSets;
        ResponsePolicies = responsePolicies;
    }

    public GcloudDnsDnsKeys DnsKeys { get; }

    public GcloudDnsManagedZones ManagedZones { get; }

    public GcloudDnsOperations Operations { get; }

    public GcloudDnsPolicies Policies { get; }

    public GcloudDnsProjectInfo ProjectInfo { get; }

    public GcloudDnsRecordSets RecordSets { get; }

    public GcloudDnsResponsePolicies ResponsePolicies { get; }
}