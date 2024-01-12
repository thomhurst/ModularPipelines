using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "roots", "delete")]
public record GcloudPrivatecaRootsDeleteOptions(
[property: PositionalArgument] string CertificateAuthority,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Pool
) : GcloudOptions
{
    [BooleanCommandSwitch("--ignore-active-certificates")]
    public bool? IgnoreActiveCertificates { get; set; }

    [BooleanCommandSwitch("--ignore-dependent-resources")]
    public bool? IgnoreDependentResources { get; set; }

    [BooleanCommandSwitch("--skip-grace-period")]
    public bool? SkipGracePeriod { get; set; }
}