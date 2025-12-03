using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "get-ca-certs")]
public record GcloudPrivatecaPoolsGetCaCertsOptions(
[property: CliArgument] string CaPool,
[property: CliArgument] string Location,
[property: CliOption("--output-file")] string OutputFile
) : GcloudOptions;