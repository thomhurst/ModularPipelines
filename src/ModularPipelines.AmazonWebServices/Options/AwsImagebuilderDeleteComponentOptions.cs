using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "delete-component")]
public record AwsImagebuilderDeleteComponentOptions(
[property: CommandSwitch("--component-build-version-arn")] string ComponentBuildVersionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}