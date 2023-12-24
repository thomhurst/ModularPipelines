using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconvert", "untag-resource")]
public record AwsMediaconvertUntagResourceOptions(
[property: CommandSwitch("--arn")] string Arn
) : AwsOptions
{
    [CommandSwitch("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}