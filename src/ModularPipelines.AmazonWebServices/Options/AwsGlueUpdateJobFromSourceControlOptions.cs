using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-job-from-source-control")]
public record AwsGlueUpdateJobFromSourceControlOptions : AwsOptions
{
    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--repository-name")]
    public string? RepositoryName { get; set; }

    [CommandSwitch("--repository-owner")]
    public string? RepositoryOwner { get; set; }

    [CommandSwitch("--branch-name")]
    public string? BranchName { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--commit-id")]
    public string? CommitId { get; set; }

    [CommandSwitch("--auth-strategy")]
    public string? AuthStrategy { get; set; }

    [CommandSwitch("--auth-token")]
    public string? AuthToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}