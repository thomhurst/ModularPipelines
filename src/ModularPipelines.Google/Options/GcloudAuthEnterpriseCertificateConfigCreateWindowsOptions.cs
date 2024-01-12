using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "enterprise-certificate-config", "create", "windows")]
public record GcloudAuthEnterpriseCertificateConfigCreateWindowsOptions(
[property: CommandSwitch("--issuer")] string Issuer,
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--store")] string Store
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