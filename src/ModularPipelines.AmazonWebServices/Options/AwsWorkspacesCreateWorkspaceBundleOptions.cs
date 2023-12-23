using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "create-workspace-bundle")]
public record AwsWorkspacesCreateWorkspaceBundleOptions(
[property: CommandSwitch("--bundle-name")] string BundleName,
[property: CommandSwitch("--bundle-description")] string BundleDescription,
[property: CommandSwitch("--image-id")] string ImageId,
[property: CommandSwitch("--compute-type")] string ComputeType,
[property: CommandSwitch("--user-storage")] string UserStorage
) : AwsOptions
{
    [CommandSwitch("--root-storage")]
    public string? RootStorage { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}