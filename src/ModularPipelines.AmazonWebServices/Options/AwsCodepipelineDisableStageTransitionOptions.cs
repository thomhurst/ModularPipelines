using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "disable-stage-transition")]
public record AwsCodepipelineDisableStageTransitionOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--transition-type")] string TransitionType,
[property: CliOption("--reason")] string Reason
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}