using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "delete-workspace-bundle")]
public record AwsWorkspacesDeleteWorkspaceBundleOptions : AwsOptions
{
    [CliOption("--bundle-id")]
    public string? BundleId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}