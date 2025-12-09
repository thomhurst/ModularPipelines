using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "create-fuota-task")]
public record AwsIotwirelessCreateFuotaTaskOptions(
[property: CliOption("--firmware-update-image")] string FirmwareUpdateImage,
[property: CliOption("--firmware-update-role")] string FirmwareUpdateRole
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

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