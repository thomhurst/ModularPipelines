using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-https-proxies", "create")]
public record GcloudComputeTargetHttpsProxiesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--url-map")] string UrlMap
) : GcloudOptions
{
    [CliOption("--certificate-map")]
    public string? CertificateMap { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--http-keep-alive-timeout-sec")]
    public string? HttpKeepAliveTimeoutSec { get; set; }

    [CliOption("--quic-override")]
    public string? QuicOverride { get; set; }

    [CliOption("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CliOption("--certificate-manager-certificates")]
    public string[]? CertificateManagerCertificates { get; set; }

    [CliOption("--ssl-certificates")]
    public string[]? SslCertificates { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--global-ssl-certificates")]
    public bool? GlobalSslCertificates { get; set; }

    [CliOption("--ssl-certificates-region")]
    public string? SslCertificatesRegion { get; set; }

    [CliFlag("--global-ssl-policy")]
    public bool? GlobalSslPolicy { get; set; }

    [CliOption("--ssl-policy-region")]
    public string? SslPolicyRegion { get; set; }

    [CliFlag("--global-url-map")]
    public bool? GlobalUrlMap { get; set; }

    [CliOption("--url-map-region")]
    public string? UrlMapRegion { get; set; }
}