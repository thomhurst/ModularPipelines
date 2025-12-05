using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "certificates", "export")]
public record GcloudPrivatecaCertificatesExportOptions(
[property: CliArgument] string Certificate,
[property: CliArgument] string IssuerLocation,
[property: CliArgument] string IssuerPool,
[property: CliOption("--output-file")] string OutputFile
) : GcloudOptions
{
    [CliFlag("--include-chain")]
    public bool? IncludeChain { get; set; }
}