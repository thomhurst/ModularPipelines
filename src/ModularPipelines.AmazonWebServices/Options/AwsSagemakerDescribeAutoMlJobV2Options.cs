using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-auto-ml-job-v2")]
public record AwsSagemakerDescribeAutoMlJobV2Options(
[property: CommandSwitch("--auto-ml-job-name")] string AutoMlJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}