using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-updated-image")]
public record AwsAppstreamCreateUpdatedImageOptions(
[property: CommandSwitch("--existing-image-name")] string ExistingImageName,
[property: CommandSwitch("--new-image-name")] string NewImageName
) : AwsOptions
{
    [CommandSwitch("--new-image-description")]
    public string? NewImageDescription { get; set; }

    [CommandSwitch("--new-image-display-name")]
    public string? NewImageDisplayName { get; set; }

    [CommandSwitch("--new-image-tags")]
    public IEnumerable<KeyValue>? NewImageTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}