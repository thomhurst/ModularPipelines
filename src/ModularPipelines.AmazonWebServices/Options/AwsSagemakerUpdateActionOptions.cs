using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-action")]
public record AwsSagemakerUpdateActionOptions(
[property: CliOption("--action-name")] string ActionName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CliOption("--properties-to-remove")]
    public string[]? PropertiesToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}