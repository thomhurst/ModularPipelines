using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("timestream-query", "execute-scheduled-query")]
public record AwsTimestreamQueryExecuteScheduledQueryOptions(
[property: CommandSwitch("--scheduled-query-arn")] string ScheduledQueryArn,
[property: CommandSwitch("--invocation-time")] long InvocationTime
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}