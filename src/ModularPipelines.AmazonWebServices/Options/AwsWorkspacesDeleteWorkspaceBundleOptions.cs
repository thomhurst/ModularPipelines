using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "delete-workspace-bundle")]
public record AwsWorkspacesDeleteWorkspaceBundleOptions : AwsOptions
{
    [CommandSwitch("--bundle-id")]
    public string? BundleId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}