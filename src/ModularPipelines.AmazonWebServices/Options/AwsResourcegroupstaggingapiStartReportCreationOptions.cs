using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resourcegroupstaggingapi", "start-report-creation")]
public record AwsResourcegroupstaggingapiStartReportCreationOptions(
[property: CliOption("--s3-bucket")] string S3Bucket
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}