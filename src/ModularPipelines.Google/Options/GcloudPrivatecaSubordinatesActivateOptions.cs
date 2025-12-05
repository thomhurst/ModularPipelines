using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "subordinates", "activate")]
public record GcloudPrivatecaSubordinatesActivateOptions(
[property: CliArgument] string CertificateAuthority,
[property: CliArgument] string Location,
[property: CliArgument] string Pool,
[property: CliOption("--pem-chain")] string PemChain
) : GcloudOptions
{
    [CliFlag("--auto-enable")]
    public bool? AutoEnable { get; set; }
}