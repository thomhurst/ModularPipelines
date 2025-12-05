using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "create-device-pool")]
public record AwsDevicefarmCreateDevicePoolOptions(
[property: CliOption("--project-arn")] string ProjectArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--max-devices")]
    public int? MaxDevices { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}