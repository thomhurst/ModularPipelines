using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "create-workspace-bundle")]
public record AwsWorkspacesCreateWorkspaceBundleOptions(
[property: CliOption("--bundle-name")] string BundleName,
[property: CliOption("--bundle-description")] string BundleDescription,
[property: CliOption("--image-id")] string ImageId,
[property: CliOption("--compute-type")] string ComputeType,
[property: CliOption("--user-storage")] string UserStorage
) : AwsOptions
{
    [CliOption("--root-storage")]
    public string? RootStorage { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}