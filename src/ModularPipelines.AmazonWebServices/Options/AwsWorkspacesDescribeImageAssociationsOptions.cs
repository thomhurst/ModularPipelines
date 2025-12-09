using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-image-associations")]
public record AwsWorkspacesDescribeImageAssociationsOptions(
[property: CliOption("--image-id")] string ImageId,
[property: CliOption("--associated-resource-types")] string[] AssociatedResourceTypes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}