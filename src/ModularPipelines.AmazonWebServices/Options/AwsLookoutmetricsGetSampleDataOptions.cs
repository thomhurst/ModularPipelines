using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "get-sample-data")]
public record AwsLookoutmetricsGetSampleDataOptions : AwsOptions
{
    [CliOption("--s3-source-config")]
    public string? S3SourceConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}