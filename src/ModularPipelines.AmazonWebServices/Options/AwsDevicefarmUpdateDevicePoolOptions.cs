using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "update-device-pool")]
public record AwsDevicefarmUpdateDevicePoolOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--rules")]
    public string[]? Rules { get; set; }

    [CliOption("--max-devices")]
    public int? MaxDevices { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}