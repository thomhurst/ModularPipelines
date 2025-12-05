using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-action")]
public record AwsSagemakerCreateActionOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--source")] string Source,
[property: CliOption("--action-type")] string ActionType
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CliOption("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}