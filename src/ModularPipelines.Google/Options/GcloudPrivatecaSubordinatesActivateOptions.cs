using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "subordinates", "activate")]
public record GcloudPrivatecaSubordinatesActivateOptions(
[property: PositionalArgument] string CertificateAuthority,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Pool,
[property: CommandSwitch("--pem-chain")] string PemChain
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-enable")]
    public bool? AutoEnable { get; set; }
}