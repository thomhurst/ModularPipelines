using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "count-pending-activity-tasks")]
public record AwsSwfCountPendingActivityTasksOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--task-list")] string TaskList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}