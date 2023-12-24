using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "delete-vod-source")]
public record AwsMediatailorDeleteVodSourceOptions(
[property: CommandSwitch("--source-location-name")] string SourceLocationName,
[property: CommandSwitch("--vod-source-name")] string VodSourceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}