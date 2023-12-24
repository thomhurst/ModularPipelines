using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "create-live-source")]
public record AwsMediatailorCreateLiveSourceOptions(
[property: CommandSwitch("--http-package-configurations")] string[] HttpPackageConfigurations,
[property: CommandSwitch("--live-source-name")] string LiveSourceName,
[property: CommandSwitch("--source-location-name")] string SourceLocationName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}