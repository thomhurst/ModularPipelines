using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "enterprise-certificate-config", "create", "linux")]
public record GcloudAuthEnterpriseCertificateConfigCreateLinuxOptions(
[property: CliOption("--label")] string Label,
[property: CliOption("--module")] string Module,
[property: CliOption("--slot")] string Slot
) : GcloudOptions
{
    [CliOption("--ecp")]
    public string? Ecp { get; set; }

    [CliOption("--ecp-client")]
    public string? EcpClient { get; set; }

    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--tls-offload")]
    public string? TlsOffload { get; set; }

    [CliOption("--user-pin")]
    public string? UserPin { get; set; }
}