using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "poll-for-activity-task")]
public record AwsSwfPollForActivityTaskOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--task-list")] string TaskList
) : AwsOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}