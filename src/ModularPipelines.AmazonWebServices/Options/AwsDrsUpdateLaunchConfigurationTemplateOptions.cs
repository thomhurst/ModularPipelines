using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "update-launch-configuration-template")]
public record AwsDrsUpdateLaunchConfigurationTemplateOptions(
[property: CommandSwitch("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CommandSwitch("--export-bucket-arn")]
    public string? ExportBucketArn { get; set; }

    [CommandSwitch("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CommandSwitch("--licensing")]
    public string? Licensing { get; set; }

    [CommandSwitch("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}