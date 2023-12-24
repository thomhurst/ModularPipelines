using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "update-workspace-bundle")]
public record AwsWorkspacesUpdateWorkspaceBundleOptions : AwsOptions
{
    [CommandSwitch("--bundle-id")]
    public string? BundleId { get; set; }

    [CommandSwitch("--image-id")]
    public string? ImageId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}