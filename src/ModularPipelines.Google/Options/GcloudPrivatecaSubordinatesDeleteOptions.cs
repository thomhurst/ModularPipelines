using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "subordinates", "delete")]
public record GcloudPrivatecaSubordinatesDeleteOptions(
[property: CliArgument] string CertificateAuthority,
[property: CliArgument] string Location,
[property: CliArgument] string Pool
) : GcloudOptions
{
    [CliFlag("--ignore-active-certificates")]
    public bool? IgnoreActiveCertificates { get; set; }

    [CliFlag("--ignore-dependent-resources")]
    public bool? IgnoreDependentResources { get; set; }

    [CliFlag("--skip-grace-period")]
    public bool? SkipGracePeriod { get; set; }
}