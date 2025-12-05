using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "update-scheduled-action")]
public record AwsRedshiftServerlessUpdateScheduledActionOptions(
[property: CliOption("--scheduled-action-name")] string ScheduledActionName
) : AwsOptions
{
    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--target-action")]
    public string? TargetAction { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}