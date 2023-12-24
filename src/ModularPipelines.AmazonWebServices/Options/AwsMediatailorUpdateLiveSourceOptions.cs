using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "update-live-source")]
public record AwsMediatailorUpdateLiveSourceOptions(
[property: CommandSwitch("--http-package-configurations")] string[] HttpPackageConfigurations,
[property: CommandSwitch("--live-source-name")] string LiveSourceName,
[property: CommandSwitch("--source-location-name")] string SourceLocationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}