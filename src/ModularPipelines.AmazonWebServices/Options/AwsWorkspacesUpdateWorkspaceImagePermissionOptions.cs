using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "update-workspace-image-permission")]
public record AwsWorkspacesUpdateWorkspaceImagePermissionOptions(
[property: CliOption("--image-id")] string ImageId,
[property: CliOption("--shared-account-id")] string SharedAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}