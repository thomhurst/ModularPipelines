using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "wait", "alarm-exists")]
public record AwsCloudwatchWaitAlarmExistsOptions : AwsOptions
{
    [CliOption("--alarm-names")]
    public string[]? AlarmNames { get; set; }

    [CliOption("--alarm-name-prefix")]
    public string? AlarmNamePrefix { get; set; }

    [CliOption("--alarm-types")]
    public string[]? AlarmTypes { get; set; }

    [CliOption("--children-of-alarm-name")]
    public string? ChildrenOfAlarmName { get; set; }

    [CliOption("--parents-of-alarm-name")]
    public string? ParentsOfAlarmName { get; set; }

    [CliOption("--state-value")]
    public string? StateValue { get; set; }

    [CliOption("--action-prefix")]
    public string? ActionPrefix { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}