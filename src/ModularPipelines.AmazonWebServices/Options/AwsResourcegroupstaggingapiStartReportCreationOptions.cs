using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resourcegroupstaggingapi", "start-report-creation")]
public record AwsResourcegroupstaggingapiStartReportCreationOptions(
[property: CommandSwitch("--s3-bucket")] string S3Bucket
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}