using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "enable-stage-transition")]
public record AwsCodepipelineEnableStageTransitionOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--transition-type")] string TransitionType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}