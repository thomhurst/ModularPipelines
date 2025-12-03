using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "enterprise-certificate-config", "create", "windows")]
public record GcloudAuthEnterpriseCertificateConfigCreateWindowsOptions(
[property: CliOption("--issuer")] string Issuer,
[property: CliOption("--provider")] string Provider,
[property: CliOption("--store")] string Store
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