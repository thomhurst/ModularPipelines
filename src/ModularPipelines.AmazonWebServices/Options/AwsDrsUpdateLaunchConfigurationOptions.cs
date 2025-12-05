using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "update-launch-configuration")]
public record AwsDrsUpdateLaunchConfigurationOptions(
[property: CliOption("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CliOption("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CliOption("--launch-into-instance-properties")]
    public string? LaunchIntoInstanceProperties { get; set; }

    [CliOption("--licensing")]
    public string? Licensing { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}