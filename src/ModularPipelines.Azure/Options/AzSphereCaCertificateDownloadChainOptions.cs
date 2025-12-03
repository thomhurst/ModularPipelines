using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "ca-certificate", "download-chain")]
public record AzSphereCaCertificateDownloadChainOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--output-file")] string OutputFile,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }
}