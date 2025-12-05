using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "certificates", "list")]
public record GcloudPrivatecaCertificatesListOptions : GcloudOptions
{
    [CliOption("--issuer-pool")]
    public string? IssuerPool { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}