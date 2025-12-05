using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-security", "get-scan")]
public record AwsCodeguruSecurityGetScanOptions(
[property: CliOption("--scan-name")] string ScanName
) : AwsOptions
{
    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}