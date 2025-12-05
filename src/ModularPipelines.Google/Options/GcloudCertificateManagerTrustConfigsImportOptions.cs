using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "trust-configs", "import")]
public record GcloudCertificateManagerTrustConfigsImportOptions(
[property: CliArgument] string TrustConfig,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }
}