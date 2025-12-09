using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-job-from-source-control")]
public record AwsGlueUpdateJobFromSourceControlOptions : AwsOptions
{
    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--repository-name")]
    public string? RepositoryName { get; set; }

    [CliOption("--repository-owner")]
    public string? RepositoryOwner { get; set; }

    [CliOption("--branch-name")]
    public string? BranchName { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--commit-id")]
    public string? CommitId { get; set; }

    [CliOption("--auth-strategy")]
    public string? AuthStrategy { get; set; }

    [CliOption("--auth-token")]
    public string? AuthToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}