using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "roots", "disable")]
public record GcloudPrivatecaRootsDisableOptions(
[property: CliArgument] string CertificateAuthority,
[property: CliArgument] string Location,
[property: CliArgument] string Pool
) : GcloudOptions
{
    [CliFlag("--ignore-dependent-resources")]
    public bool? IgnoreDependentResources { get; set; }
}