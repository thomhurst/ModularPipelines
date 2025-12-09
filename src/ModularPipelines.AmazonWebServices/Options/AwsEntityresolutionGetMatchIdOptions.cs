using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "get-match-id")]
public record AwsEntityresolutionGetMatchIdOptions(
[property: CliOption("--record")] IEnumerable<KeyValue> Record,
[property: CliOption("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}