using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "describe-bundle-associations")]
public record AwsWorkspacesDescribeBundleAssociationsOptions(
[property: CommandSwitch("--bundle-id")] string BundleId,
[property: CommandSwitch("--associated-resource-types")] string[] AssociatedResourceTypes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}