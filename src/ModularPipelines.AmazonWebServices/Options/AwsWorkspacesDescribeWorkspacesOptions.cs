using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-workspaces")]
public record AwsWorkspacesDescribeWorkspacesOptions : AwsOptions
{
    [CliOption("--workspace-ids")]
    public string[]? WorkspaceIds { get; set; }

    [CliOption("--directory-id")]
    public string? DirectoryId { get; set; }

    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--bundle-id")]
    public string? BundleId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}