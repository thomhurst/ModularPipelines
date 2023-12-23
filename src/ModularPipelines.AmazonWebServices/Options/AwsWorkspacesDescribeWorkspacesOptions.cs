using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "describe-workspaces")]
public record AwsWorkspacesDescribeWorkspacesOptions : AwsOptions
{
    [CommandSwitch("--workspace-ids")]
    public string[]? WorkspaceIds { get; set; }

    [CommandSwitch("--directory-id")]
    public string? DirectoryId { get; set; }

    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--bundle-id")]
    public string? BundleId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}