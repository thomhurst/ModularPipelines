using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-roborunner", "create-destination")]
public record AwsIotRoborunnerCreateDestinationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--site")] string Site
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--additional-fixed-properties")]
    public string? AdditionalFixedProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}