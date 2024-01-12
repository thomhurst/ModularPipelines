using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-https-proxies", "update")]
public record GcloudComputeTargetHttpsProxiesUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--quic-override")]
    public string? QuicOverride { get; set; }

    [CommandSwitch("--url-map")]
    public string? UrlMap { get; set; }

    [CommandSwitch("--certificate-manager-certificates")]
    public string[]? CertificateManagerCertificates { get; set; }

    [BooleanCommandSwitch("--clear-ssl-certificates")]
    public bool? ClearSslCertificates { get; set; }

    [CommandSwitch("--ssl-certificates")]
    public string[]? SslCertificates { get; set; }

    [BooleanCommandSwitch("--global-ssl-certificates")]
    public bool? GlobalSslCertificates { get; set; }

    [CommandSwitch("--ssl-certificates-region")]
    public string? SslCertificatesRegion { get; set; }

    [CommandSwitch("--certificate-map")]
    public string? CertificateMap { get; set; }

    [BooleanCommandSwitch("--clear-certificate-map")]
    public bool? ClearCertificateMap { get; set; }

    [BooleanCommandSwitch("--clear-http-keep-alive-timeout-sec")]
    public bool? ClearHttpKeepAliveTimeoutSec { get; set; }

    [CommandSwitch("--http-keep-alive-timeout-sec")]
    public string? HttpKeepAliveTimeoutSec { get; set; }

    [BooleanCommandSwitch("--clear-ssl-policy")]
    public bool? ClearSslPolicy { get; set; }

    [CommandSwitch("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [BooleanCommandSwitch("--global-ssl-policy")]
    public bool? GlobalSslPolicy { get; set; }

    [CommandSwitch("--ssl-policy-region")]
    public string? SslPolicyRegion { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--global-url-map")]
    public bool? GlobalUrlMap { get; set; }

    [CommandSwitch("--url-map-region")]
    public string? UrlMapRegion { get; set; }
}