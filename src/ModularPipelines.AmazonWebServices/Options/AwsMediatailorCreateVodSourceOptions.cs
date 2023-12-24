using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "create-vod-source")]
public record AwsMediatailorCreateVodSourceOptions(
[property: CommandSwitch("--http-package-configurations")] string[] HttpPackageConfigurations,
[property: CommandSwitch("--source-location-name")] string SourceLocationName,
[property: CommandSwitch("--vod-source-name")] string VodSourceName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}