using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-bundle-associations")]
public record AwsWorkspacesDescribeBundleAssociationsOptions(
[property: CliOption("--bundle-id")] string BundleId,
[property: CliOption("--associated-resource-types")] string[] AssociatedResourceTypes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}