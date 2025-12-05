using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "update-workspace-bundle")]
public record AwsWorkspacesUpdateWorkspaceBundleOptions : AwsOptions
{
    [CliOption("--bundle-id")]
    public string? BundleId { get; set; }

    [CliOption("--image-id")]
    public string? ImageId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}