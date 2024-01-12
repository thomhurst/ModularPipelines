using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "pools", "get-ca-certs")]
public record GcloudPrivatecaPoolsGetCaCertsOptions(
[property: PositionalArgument] string CaPool,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--output-file")] string OutputFile
) : GcloudOptions;