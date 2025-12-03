using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "put-workflow-run-properties")]
public record AwsGluePutWorkflowRunPropertiesOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--run-id")] string RunId,
[property: CliOption("--run-properties")] IEnumerable<KeyValue> RunProperties
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}