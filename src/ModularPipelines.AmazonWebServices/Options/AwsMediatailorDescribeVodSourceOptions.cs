using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "describe-vod-source")]
public record AwsMediatailorDescribeVodSourceOptions(
[property: CliOption("--source-location-name")] string SourceLocationName,
[property: CliOption("--vod-source-name")] string VodSourceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}