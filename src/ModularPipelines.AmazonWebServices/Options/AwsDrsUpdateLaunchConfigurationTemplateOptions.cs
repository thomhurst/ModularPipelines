using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "update-launch-configuration-template")]
public record AwsDrsUpdateLaunchConfigurationTemplateOptions(
[property: CliOption("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CliOption("--export-bucket-arn")]
    public string? ExportBucketArn { get; set; }

    [CliOption("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CliOption("--licensing")]
    public string? Licensing { get; set; }

    [CliOption("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}