using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "roots", "undelete")]
public record GcloudPrivatecaRootsUndeleteOptions(
[property: PositionalArgument] string CertificateAuthority,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Pool
) : GcloudOptions;