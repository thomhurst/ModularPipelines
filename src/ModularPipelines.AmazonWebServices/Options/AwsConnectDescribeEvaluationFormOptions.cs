using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-evaluation-form")]
public record AwsConnectDescribeEvaluationFormOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--evaluation-form-id")] string EvaluationFormId
) : AwsOptions
{
    [CommandSwitch("--evaluation-form-version")]
    public int? EvaluationFormVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}