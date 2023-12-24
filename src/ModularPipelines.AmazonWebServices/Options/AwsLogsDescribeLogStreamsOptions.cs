using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "describe-log-streams")]
public record AwsLogsDescribeLogStreamsOptions : AwsOptions
{
    [CommandSwitch("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CommandSwitch("--log-group-identifier")]
    public string? LogGroupIdentifier { get; set; }

    [CommandSwitch("--log-stream-name-prefix")]
    public string? LogStreamNamePrefix { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}