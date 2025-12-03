using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "resume-workflow-run")]
public record AwsGlueResumeWorkflowRunOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--run-id")] string RunId,
[property: CliOption("--node-ids")] string[] NodeIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}