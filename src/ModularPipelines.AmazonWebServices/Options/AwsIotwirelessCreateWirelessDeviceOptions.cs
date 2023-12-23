using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "create-wireless-device")]
public record AwsIotwirelessCreateWirelessDeviceOptions(
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--destination-name")] string DestinationName
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--positioning")]
    public string? Positioning { get; set; }

    [CommandSwitch("--sidewalk")]
    public string? Sidewalk { get; set; }

    [CommandSwitch("--lorawan")]
    public string? Lorawan { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}