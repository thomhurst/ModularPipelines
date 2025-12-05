using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-selfservice-permissions")]
public record AwsWorkspacesModifySelfservicePermissionsOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--selfservice-permissions")] string SelfservicePermissions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}