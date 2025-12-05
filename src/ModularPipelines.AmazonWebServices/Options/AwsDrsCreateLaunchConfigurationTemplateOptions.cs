using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "create-launch-configuration-template")]
public record AwsDrsCreateLaunchConfigurationTemplateOptions : AwsOptions
{
    [CliOption("--export-bucket-arn")]
    public string? ExportBucketArn { get; set; }

    [CliOption("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CliOption("--licensing")]
    public string? Licensing { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}