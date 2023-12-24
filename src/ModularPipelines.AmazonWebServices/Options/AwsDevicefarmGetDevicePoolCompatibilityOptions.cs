using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "get-device-pool-compatibility")]
public record AwsDevicefarmGetDevicePoolCompatibilityOptions(
[property: CommandSwitch("--device-pool-arn")] string DevicePoolArn
) : AwsOptions
{
    [CommandSwitch("--app-arn")]
    public string? AppArn { get; set; }

    [CommandSwitch("--test-type")]
    public string? TestType { get; set; }

    [CommandSwitch("--test")]
    public string? Test { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}