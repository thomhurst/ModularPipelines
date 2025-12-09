using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "associate-workspace-application")]
public record AwsWorkspacesAssociateWorkspaceApplicationOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}