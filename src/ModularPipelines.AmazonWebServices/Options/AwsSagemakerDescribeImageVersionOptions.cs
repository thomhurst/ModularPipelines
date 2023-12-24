using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-image-version")]
public record AwsSagemakerDescribeImageVersionOptions(
[property: CommandSwitch("--image-name")] string ImageName
) : AwsOptions
{
    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--version-number")]
    public int? VersionNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}