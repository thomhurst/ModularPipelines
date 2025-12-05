using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-workflow-run")]
public record AwsGlueStartWorkflowRunOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--run-properties")]
    public IEnumerable<KeyValue>? RunProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}