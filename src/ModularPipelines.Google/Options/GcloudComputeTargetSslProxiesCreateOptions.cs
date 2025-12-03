using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-ssl-proxies", "create")]
public record GcloudComputeTargetSslProxiesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--backend-service")] string BackendService,
[property: CliOption("--certificate-map")] string CertificateMap,
[property: CliOption("--ssl-certificates")] string[] SslCertificates
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [CliOption("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CliFlag("--global-ssl-policy")]
    public bool? GlobalSslPolicy { get; set; }

    [CliOption("--ssl-policy-region")]
    public string? SslPolicyRegion { get; set; }
}