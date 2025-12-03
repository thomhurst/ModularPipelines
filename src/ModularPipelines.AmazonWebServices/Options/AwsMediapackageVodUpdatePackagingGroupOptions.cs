using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage-vod", "update-packaging-group")]
public record AwsMediapackageVodUpdatePackagingGroupOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--authorization")]
    public string? Authorization { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}