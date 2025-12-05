using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-query", "describe-scheduled-query")]
public record AwsTimestreamQueryDescribeScheduledQueryOptions(
[property: CliOption("--scheduled-query-arn")] string ScheduledQueryArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}