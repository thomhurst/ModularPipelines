using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "roots", "disable")]
public record GcloudPrivatecaRootsDisableOptions(
[property: PositionalArgument] string CertificateAuthority,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Pool
) : GcloudOptions
{
    [BooleanCommandSwitch("--ignore-dependent-resources")]
    public bool? IgnoreDependentResources { get; set; }
}