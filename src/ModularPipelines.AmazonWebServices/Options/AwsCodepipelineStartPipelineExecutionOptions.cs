using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "start-pipeline-execution")]
public record AwsCodepipelineStartPipelineExecutionOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--variables")]
    public string[]? Variables { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--source-revisions")]
    public string[]? SourceRevisions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}