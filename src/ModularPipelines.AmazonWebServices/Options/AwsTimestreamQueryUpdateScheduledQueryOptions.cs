using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-query", "update-scheduled-query")]
public record AwsTimestreamQueryUpdateScheduledQueryOptions(
[property: CliOption("--scheduled-query-arn")] string ScheduledQueryArn,
[property: CliOption("--state")] string State
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}