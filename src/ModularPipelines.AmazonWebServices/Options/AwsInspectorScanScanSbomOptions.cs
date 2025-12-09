using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector-scan", "scan-sbom")]
public record AwsInspectorScanScanSbomOptions(
[property: CliOption("--sbom")] string Sbom
) : AwsOptions
{
    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}