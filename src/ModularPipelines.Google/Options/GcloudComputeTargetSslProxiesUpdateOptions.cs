using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-ssl-proxies", "update")]
public record GcloudComputeTargetSslProxiesUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--backend-service")]
    public string? BackendService { get; set; }

    [CommandSwitch("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [BooleanCommandSwitch("--clear-ssl-policy")]
    public bool? ClearSslPolicy { get; set; }

    [CommandSwitch("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [BooleanCommandSwitch("--global-ssl-policy")]
    public bool? GlobalSslPolicy { get; set; }

    [CommandSwitch("--ssl-policy-region")]
    public string? SslPolicyRegion { get; set; }

    [CommandSwitch("--ssl-certificates")]
    public string[]? SslCertificates { get; set; }

    [BooleanCommandSwitch("--clear-ssl-certificates")]
    public bool? ClearSslCertificates { get; set; }

    [CommandSwitch("--certificate-map")]
    public string? CertificateMap { get; set; }

    [BooleanCommandSwitch("--clear-certificate-map")]
    public bool? ClearCertificateMap { get; set; }
}