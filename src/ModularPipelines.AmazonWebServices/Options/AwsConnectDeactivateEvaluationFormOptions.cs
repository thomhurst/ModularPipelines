using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "deactivate-evaluation-form")]
public record AwsConnectDeactivateEvaluationFormOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--evaluation-form-id")] string EvaluationFormId,
[property: CommandSwitch("--evaluation-form-version")] int EvaluationFormVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}