using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "trust-configs", "delete")]
public record GcloudCertificateManagerTrustConfigsDeleteOptions(
[property: CliArgument] string TrustConfig,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}