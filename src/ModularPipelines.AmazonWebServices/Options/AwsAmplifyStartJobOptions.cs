using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "start-job")]
public record AwsAmplifyStartJobOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--branch-name")] string BranchName,
[property: CliOption("--job-type")] string JobType
) : AwsOptions
{
    [CliOption("--job-id")]
    public string? JobId { get; set; }

    [CliOption("--job-reason")]
    public string? JobReason { get; set; }

    [CliOption("--commit-id")]
    public string? CommitId { get; set; }

    [CliOption("--commit-message")]
    public string? CommitMessage { get; set; }

    [CliOption("--commit-time")]
    public long? CommitTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}