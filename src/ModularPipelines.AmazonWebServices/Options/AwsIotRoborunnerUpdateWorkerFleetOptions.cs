using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-roborunner", "update-worker-fleet")]
public record AwsIotRoborunnerUpdateWorkerFleetOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--additional-fixed-properties")]
    public string? AdditionalFixedProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}