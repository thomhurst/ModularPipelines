using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "start-job")]
public record AwsAmplifyStartJobOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--branch-name")] string BranchName,
[property: CommandSwitch("--job-type")] string JobType
) : AwsOptions
{
    [CommandSwitch("--job-id")]
    public string? JobId { get; set; }

    [CommandSwitch("--job-reason")]
    public string? JobReason { get; set; }

    [CommandSwitch("--commit-id")]
    public string? CommitId { get; set; }

    [CommandSwitch("--commit-message")]
    public string? CommitMessage { get; set; }

    [CommandSwitch("--commit-time")]
    public long? CommitTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}