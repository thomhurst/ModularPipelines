using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "describe-model-packaging-job")]
public record AwsLookoutvisionDescribeModelPackagingJobOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--job-name")] string JobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}