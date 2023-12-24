using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "batch-get-device-position")]
public record AwsLocationBatchGetDevicePositionOptions(
[property: CommandSwitch("--device-ids")] string[] DeviceIds,
[property: CommandSwitch("--tracker-name")] string TrackerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}