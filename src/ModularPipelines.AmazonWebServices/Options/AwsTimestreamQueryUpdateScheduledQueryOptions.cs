using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("timestream-query", "update-scheduled-query")]
public record AwsTimestreamQueryUpdateScheduledQueryOptions(
[property: CommandSwitch("--scheduled-query-arn")] string ScheduledQueryArn,
[property: CommandSwitch("--state")] string State
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}