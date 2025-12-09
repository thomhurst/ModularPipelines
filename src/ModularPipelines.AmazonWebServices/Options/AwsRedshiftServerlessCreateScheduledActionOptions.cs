using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "create-scheduled-action")]
public record AwsRedshiftServerlessCreateScheduledActionOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--schedule")] string Schedule,
[property: CliOption("--scheduled-action-name")] string ScheduledActionName,
[property: CliOption("--target-action")] string TargetAction
) : AwsOptions
{
    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}