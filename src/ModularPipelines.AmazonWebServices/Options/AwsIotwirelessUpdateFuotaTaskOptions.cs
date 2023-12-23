using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-fuota-task")]
public record AwsIotwirelessUpdateFuotaTaskOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--firmware-update-image")]
    public string? FirmwareUpdateImage { get; set; }

    [CommandSwitch("--firmware-update-role")]
    public string? FirmwareUpdateRole { get; set; }

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