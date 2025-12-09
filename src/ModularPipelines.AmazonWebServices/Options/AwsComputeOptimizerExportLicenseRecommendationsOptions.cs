using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "export-license-recommendations")]
public record AwsComputeOptimizerExportLicenseRecommendationsOptions(
[property: CliOption("--s3-destination-config")] string S3DestinationConfig
) : AwsOptions
{
    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--fields-to-export")]
    public string[]? FieldsToExport { get; set; }

    [CliOption("--file-format")]
    public string? FileFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}