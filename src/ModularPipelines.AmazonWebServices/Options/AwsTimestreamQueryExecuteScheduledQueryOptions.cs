using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-query", "execute-scheduled-query")]
public record AwsTimestreamQueryExecuteScheduledQueryOptions(
[property: CliOption("--scheduled-query-arn")] string ScheduledQueryArn,
[property: CliOption("--invocation-time")] long InvocationTime
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}