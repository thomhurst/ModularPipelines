using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-disk")]
public record AwsLightsailCreateDiskOptions(
[property: CliOption("--disk-name")] string DiskName,
[property: CliOption("--availability-zone")] string AvailabilityZone,
[property: CliOption("--size-in-gb")] int SizeInGb
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--add-ons")]
    public string[]? AddOns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}