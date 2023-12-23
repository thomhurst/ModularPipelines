using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "copy-image")]
public record AwsAppstreamCopyImageOptions(
[property: CommandSwitch("--source-image-name")] string SourceImageName,
[property: CommandSwitch("--destination-image-name")] string DestinationImageName,
[property: CommandSwitch("--destination-region")] string DestinationRegion
) : AwsOptions
{
    [CommandSwitch("--destination-image-description")]
    public string? DestinationImageDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}