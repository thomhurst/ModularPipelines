using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-jobs-data", "get-pending-job-executions")]
public record AwsIotJobsDataGetPendingJobExecutionsOptions(
[property: CommandSwitch("--thing-name")] string ThingName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}