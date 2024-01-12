using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "enterprise-certificate-config", "create", "linux")]
public record GcloudAuthEnterpriseCertificateConfigCreateLinuxOptions(
[property: CommandSwitch("--label")] string Label,
[property: CommandSwitch("--module")] string Module,
[property: CommandSwitch("--slot")] string Slot
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

    [CommandSwitch("--user-pin")]
    public string? UserPin { get; set; }
}