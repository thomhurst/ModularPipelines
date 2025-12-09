using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "update-live-source")]
public record AwsMediatailorUpdateLiveSourceOptions(
[property: CliOption("--http-package-configurations")] string[] HttpPackageConfigurations,
[property: CliOption("--live-source-name")] string LiveSourceName,
[property: CliOption("--source-location-name")] string SourceLocationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}