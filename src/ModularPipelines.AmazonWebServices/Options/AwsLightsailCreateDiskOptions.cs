using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-disk")]
public record AwsLightsailCreateDiskOptions(
[property: CommandSwitch("--disk-name")] string DiskName,
[property: CommandSwitch("--availability-zone")] string AvailabilityZone,
[property: CommandSwitch("--size-in-gb")] int SizeInGb
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--add-ons")]
    public string[]? AddOns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}