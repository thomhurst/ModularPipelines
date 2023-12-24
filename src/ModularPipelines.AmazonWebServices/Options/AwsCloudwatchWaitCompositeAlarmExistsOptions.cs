using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "wait", "composite-alarm-exists")]
public record AwsCloudwatchWaitCompositeAlarmExistsOptions : AwsOptions
{
    [CommandSwitch("--alarm-names")]
    public string[]? AlarmNames { get; set; }

    [CommandSwitch("--alarm-name-prefix")]
    public string? AlarmNamePrefix { get; set; }

    [CommandSwitch("--alarm-types")]
    public string[]? AlarmTypes { get; set; }

    [CommandSwitch("--children-of-alarm-name")]
    public string? ChildrenOfAlarmName { get; set; }

    [CommandSwitch("--parents-of-alarm-name")]
    public string? ParentsOfAlarmName { get; set; }

    [CommandSwitch("--state-value")]
    public string? StateValue { get; set; }

    [CommandSwitch("--action-prefix")]
    public string? ActionPrefix { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}