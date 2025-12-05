using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "migrate-workspace")]
public record AwsWorkspacesMigrateWorkspaceOptions(
[property: CliOption("--source-workspace-id")] string SourceWorkspaceId,
[property: CliOption("--bundle-id")] string BundleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}