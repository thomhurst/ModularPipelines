using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "delete-image")]
public record AwsImagebuilderDeleteImageOptions(
[property: CommandSwitch("--image-build-version-arn")] string ImageBuildVersionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}