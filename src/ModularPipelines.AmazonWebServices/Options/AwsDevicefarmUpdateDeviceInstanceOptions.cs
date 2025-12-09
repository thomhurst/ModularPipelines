using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "update-device-instance")]
public record AwsDevicefarmUpdateDeviceInstanceOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--profile-arn")]
    public string? ProfileArn { get; set; }

    [CliOption("--labels")]
    public string[]? Labels { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}