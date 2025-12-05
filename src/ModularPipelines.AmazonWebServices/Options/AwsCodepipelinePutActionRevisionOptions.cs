using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "put-action-revision")]
public record AwsCodepipelinePutActionRevisionOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--action-revision")] string ActionRevision
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}