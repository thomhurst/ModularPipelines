using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-evaluation-form")]
public record AwsConnectUpdateEvaluationFormOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--evaluation-form-id")] string EvaluationFormId,
[property: CommandSwitch("--evaluation-form-version")] int EvaluationFormVersion,
[property: CommandSwitch("--title")] string Title,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--scoring-strategy")]
    public string? ScoringStrategy { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}