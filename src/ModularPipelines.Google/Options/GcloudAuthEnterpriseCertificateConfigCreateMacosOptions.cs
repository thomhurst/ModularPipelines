using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "enterprise-certificate-config", "create", "macos")]
public record GcloudAuthEnterpriseCertificateConfigCreateMacosOptions(
[property: CommandSwitch("--issuer")] string Issuer
) : GcloudOptions
{
    [CommandSwitch("--ecp")]
    public string? Ecp { get; set; }

    [CommandSwitch("--ecp-client")]
    public string? EcpClient { get; set; }

    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }

    [CommandSwitch("--tls-offload")]
    public string? TlsOffload { get; set; }
}