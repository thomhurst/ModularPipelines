using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "enable-stage-transition")]
public record AwsCodepipelineEnableStageTransitionOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--stage-name")] string StageName,
[property: CommandSwitch("--transition-type")] string TransitionType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}