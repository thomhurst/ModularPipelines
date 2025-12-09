using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-workspace-directories")]
public record AwsWorkspacesDescribeWorkspaceDirectoriesOptions : AwsOptions
{
    [CliOption("--directory-ids")]
    public string[]? DirectoryIds { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}