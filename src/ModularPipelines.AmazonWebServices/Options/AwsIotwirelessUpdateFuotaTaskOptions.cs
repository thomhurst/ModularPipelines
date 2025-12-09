using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-fuota-task")]
public record AwsIotwirelessUpdateFuotaTaskOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--firmware-update-image")]
    public string? FirmwareUpdateImage { get; set; }

    [CliOption("--firmware-update-role")]
    public string? FirmwareUpdateRole { get; set; }

    [CliOption("--redundancy-percent")]
    public int? RedundancyPercent { get; set; }

    [CliOption("--fragment-size-bytes")]
    public int? FragmentSizeBytes { get; set; }

    [CliOption("--fragment-interval-ms")]
    public int? FragmentIntervalMs { get; set; }

    [CliOption("--lorawan")]
    public string? Lorawan { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}