using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "certificates", "revoke")]
public record GcloudPrivatecaCertificatesRevokeOptions(
[property: CliOption("--certificate")] string Certificate,
[property: CliOption("--serial-number")] string SerialNumber
) : GcloudOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--issuer-pool")]
    public string? IssuerPool { get; set; }

    [CliOption("--issuer-location")]
    public string? IssuerLocation { get; set; }
}