using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "put-action-revision")]
public record AwsCodepipelinePutActionRevisionOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--stage-name")] string StageName,
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--action-revision")] string ActionRevision
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}