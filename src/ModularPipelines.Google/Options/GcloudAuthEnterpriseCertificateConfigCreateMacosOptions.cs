using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "enterprise-certificate-config", "create", "macos")]
public record GcloudAuthEnterpriseCertificateConfigCreateMacosOptions(
[property: CliOption("--issuer")] string Issuer
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
}