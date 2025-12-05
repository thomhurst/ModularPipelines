using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-https-proxies", "update")]
public record GcloudComputeTargetHttpsProxiesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--quic-override")]
    public string? QuicOverride { get; set; }

    [CliOption("--url-map")]
    public string? UrlMap { get; set; }

    [CliOption("--certificate-manager-certificates")]
    public string[]? CertificateManagerCertificates { get; set; }

    [CliFlag("--clear-ssl-certificates")]
    public bool? ClearSslCertificates { get; set; }

    [CliOption("--ssl-certificates")]
    public string[]? SslCertificates { get; set; }

    [CliFlag("--global-ssl-certificates")]
    public bool? GlobalSslCertificates { get; set; }

    [CliOption("--ssl-certificates-region")]
    public string? SslCertificatesRegion { get; set; }

    [CliOption("--certificate-map")]
    public string? CertificateMap { get; set; }

    [CliFlag("--clear-certificate-map")]
    public bool? ClearCertificateMap { get; set; }

    [CliFlag("--clear-http-keep-alive-timeout-sec")]
    public bool? ClearHttpKeepAliveTimeoutSec { get; set; }

    [CliOption("--http-keep-alive-timeout-sec")]
    public string? HttpKeepAliveTimeoutSec { get; set; }

    [CliFlag("--clear-ssl-policy")]
    public bool? ClearSslPolicy { get; set; }

    [CliOption("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CliFlag("--global-ssl-policy")]
    public bool? GlobalSslPolicy { get; set; }

    [CliOption("--ssl-policy-region")]
    public string? SslPolicyRegion { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--global-url-map")]
    public bool? GlobalUrlMap { get; set; }

    [CliOption("--url-map-region")]
    public string? UrlMapRegion { get; set; }
}