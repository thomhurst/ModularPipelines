using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "ca-certificate", "download-proof")]
public record AzSphereCaCertificateDownloadProofOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--output-file")] string OutputFile,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--verification-code")] string VerificationCode
) : AzOptions
{
    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }
}