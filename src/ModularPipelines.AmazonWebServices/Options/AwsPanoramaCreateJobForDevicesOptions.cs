using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "create-job-for-devices")]
public record AwsPanoramaCreateJobForDevicesOptions(
[property: CommandSwitch("--device-ids")] string[] DeviceIds,
[property: CommandSwitch("--job-type")] string JobType
) : AwsOptions
{
    [CommandSwitch("--device-job-config")]
    public string? DeviceJobConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}