using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-ssl-proxies", "create")]
public record GcloudComputeTargetSslProxiesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--backend-service")] string BackendService,
[property: CommandSwitch("--certificate-map")] string CertificateMap,
[property: CommandSwitch("--ssl-certificates")] string[] SslCertificates
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [CommandSwitch("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [BooleanCommandSwitch("--global-ssl-policy")]
    public bool? GlobalSslPolicy { get; set; }

    [CommandSwitch("--ssl-policy-region")]
    public string? SslPolicyRegion { get; set; }
}