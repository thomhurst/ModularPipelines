using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "create-fuota-task")]
public record AwsIotwirelessCreateFuotaTaskOptions(
[property: CommandSwitch("--firmware-update-image")] string FirmwareUpdateImage,
[property: CommandSwitch("--firmware-update-role")] string FirmwareUpdateRole
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

    [CommandSwitch("--redundancy-percent")]
    public int? RedundancyPercent { get; set; }

    [CommandSwitch("--fragment-size-bytes")]
    public int? FragmentSizeBytes { get; set; }

    [CommandSwitch("--fragment-interval-ms")]
    public int? FragmentIntervalMs { get; set; }

    [CommandSwitch("--lorawan")]
    public string? Lorawan { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}