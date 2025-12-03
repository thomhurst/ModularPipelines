using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "update-vod-source")]
public record AwsMediatailorUpdateVodSourceOptions(
[property: CliOption("--http-package-configurations")] string[] HttpPackageConfigurations,
[property: CliOption("--source-location-name")] string SourceLocationName,
[property: CliOption("--vod-source-name")] string VodSourceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}