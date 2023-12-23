using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "describe-image-associations")]
public record AwsWorkspacesDescribeImageAssociationsOptions(
[property: CommandSwitch("--image-id")] string ImageId,
[property: CommandSwitch("--associated-resource-types")] string[] AssociatedResourceTypes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}