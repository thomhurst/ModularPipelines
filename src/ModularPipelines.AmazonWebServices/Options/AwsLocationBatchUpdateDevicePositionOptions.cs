using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "batch-update-device-position")]
public record AwsLocationBatchUpdateDevicePositionOptions(
[property: CommandSwitch("--tracker-name")] string TrackerName,
[property: CommandSwitch("--updates")] string[] Updates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}