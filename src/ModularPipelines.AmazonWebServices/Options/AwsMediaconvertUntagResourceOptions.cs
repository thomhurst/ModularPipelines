using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconvert", "untag-resource")]
public record AwsMediaconvertUntagResourceOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}