using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rum", "put-rum-metrics-destination")]
public record AwsRumPutRumMetricsDestinationOptions(
[property: CommandSwitch("--app-monitor-name")] string AppMonitorName,
[property: CommandSwitch("--destination")] string Destination
) : AwsOptions
{
    [CommandSwitch("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CommandSwitch("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}