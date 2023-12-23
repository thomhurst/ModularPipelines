using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "put-workflow-run-properties")]
public record AwsGluePutWorkflowRunPropertiesOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--run-id")] string RunId,
[property: CommandSwitch("--run-properties")] IEnumerable<KeyValue> RunProperties
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}