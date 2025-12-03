using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-query", "delete-scheduled-query")]
public record AwsTimestreamQueryDeleteScheduledQueryOptions(
[property: CliOption("--scheduled-query-arn")] string ScheduledQueryArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}