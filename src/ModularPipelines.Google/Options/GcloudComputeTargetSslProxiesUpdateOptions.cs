using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-ssl-proxies", "update")]
public record GcloudComputeTargetSslProxiesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--backend-service")]
    public string? BackendService { get; set; }

    [CliOption("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [CliFlag("--clear-ssl-policy")]
    public bool? ClearSslPolicy { get; set; }

    [CliOption("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CliFlag("--global-ssl-policy")]
    public bool? GlobalSslPolicy { get; set; }

    [CliOption("--ssl-policy-region")]
    public string? SslPolicyRegion { get; set; }

    [CliOption("--ssl-certificates")]
    public string[]? SslCertificates { get; set; }

    [CliFlag("--clear-ssl-certificates")]
    public bool? ClearSslCertificates { get; set; }

    [CliOption("--certificate-map")]
    public string? CertificateMap { get; set; }

    [CliFlag("--clear-certificate-map")]
    public bool? ClearCertificateMap { get; set; }
}