using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-detect-mitigation-actions-executions")]
public record AwsIotListDetectMitigationActionsExecutionsOptions : AwsOptions
{
    [CommandSwitch("--task-id")]
    public string? TaskId { get; set; }

    [CommandSwitch("--violation-id")]
    public string? ViolationId { get; set; }

    [CommandSwitch("--thing-name")]
    public string? ThingName { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}