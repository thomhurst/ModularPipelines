using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "certificates", "export")]
public record GcloudPrivatecaCertificatesExportOptions(
[property: PositionalArgument] string Certificate,
[property: PositionalArgument] string IssuerLocation,
[property: PositionalArgument] string IssuerPool,
[property: CommandSwitch("--output-file")] string OutputFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--include-chain")]
    public bool? IncludeChain { get; set; }
}