using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "activate-evaluation-form")]
public record AwsConnectActivateEvaluationFormOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--evaluation-form-id")] string EvaluationFormId,
[property: CommandSwitch("--evaluation-form-version")] int EvaluationFormVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}