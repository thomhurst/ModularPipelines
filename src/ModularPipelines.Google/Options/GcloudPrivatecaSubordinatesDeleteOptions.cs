using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "subordinates", "delete")]
public record GcloudPrivatecaSubordinatesDeleteOptions(
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