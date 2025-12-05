using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "poll-for-decision-task")]
public record AwsSwfPollForDecisionTaskOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--task-list")] string TaskList
) : AwsOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}