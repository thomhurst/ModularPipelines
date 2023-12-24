using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-roborunner", "update-worker")]
public record AwsIotRoborunnerUpdateWorkerOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--additional-transient-properties")]
    public string? AdditionalTransientProperties { get; set; }

    [CommandSwitch("--additional-fixed-properties")]
    public string? AdditionalFixedProperties { get; set; }

    [CommandSwitch("--vendor-properties")]
    public string? VendorProperties { get; set; }

    [CommandSwitch("--position")]
    public string? Position { get; set; }

    [CommandSwitch("--orientation")]
    public string? Orientation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}