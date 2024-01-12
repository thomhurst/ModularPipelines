using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "roots", "enable")]
public record GcloudPrivatecaRootsEnableOptions(
[property: PositionalArgument] string CertificateAuthority,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Pool
) : GcloudOptions;