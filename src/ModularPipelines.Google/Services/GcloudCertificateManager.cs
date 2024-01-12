using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudCertificateManager
{
    public GcloudCertificateManager(
        GcloudCertificateManagerCertificates certificates,
        GcloudCertificateManagerDnsAuthorizations dnsAuthorizations,
        GcloudCertificateManagerIssuanceConfigs issuanceConfigs,
        GcloudCertificateManagerMaps maps,
        GcloudCertificateManagerOperations operations,
        GcloudCertificateManagerTrustConfigs trustConfigs
    )
    {
        Certificates = certificates;
        DnsAuthorizations = dnsAuthorizations;
        IssuanceConfigs = issuanceConfigs;
        Maps = maps;
        Operations = operations;
        TrustConfigs = trustConfigs;
    }

    public GcloudCertificateManagerCertificates Certificates { get; }

    public GcloudCertificateManagerDnsAuthorizations DnsAuthorizations { get; }

    public GcloudCertificateManagerIssuanceConfigs IssuanceConfigs { get; }

    public GcloudCertificateManagerMaps Maps { get; }

    public GcloudCertificateManagerOperations Operations { get; }

    public GcloudCertificateManagerTrustConfigs TrustConfigs { get; }
}