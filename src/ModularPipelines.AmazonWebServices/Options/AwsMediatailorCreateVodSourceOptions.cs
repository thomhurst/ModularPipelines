using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "create-vod-source")]
public record AwsMediatailorCreateVodSourceOptions(
[property: CliOption("--http-package-configurations")] string[] HttpPackageConfigurations,
[property: CliOption("--source-location-name")] string SourceLocationName,
[property: CliOption("--vod-source-name")] string VodSourceName
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}