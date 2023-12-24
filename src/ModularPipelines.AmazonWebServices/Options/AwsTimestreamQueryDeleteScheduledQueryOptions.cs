using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("timestream-query", "delete-scheduled-query")]
public record AwsTimestreamQueryDeleteScheduledQueryOptions(
[property: CommandSwitch("--scheduled-query-arn")] string ScheduledQueryArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}