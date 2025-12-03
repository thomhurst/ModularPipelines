using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "discover-input-schema")]
public record AwsKinesisanalyticsv2DiscoverInputSchemaOptions(
[property: CliOption("--service-execution-role")] string ServiceExecutionRole
) : AwsOptions
{
    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--input-starting-position-configuration")]
    public string? InputStartingPositionConfiguration { get; set; }

    [CliOption("--s3-configuration")]
    public string? S3Configuration { get; set; }

    [CliOption("--input-processing-configuration")]
    public string? InputProcessingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}