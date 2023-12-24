using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "describe-step")]
public record AwsEmrDescribeStepOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--step-id")] string StepId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}