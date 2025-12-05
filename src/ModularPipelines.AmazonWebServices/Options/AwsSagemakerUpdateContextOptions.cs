using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-context")]
public record AwsSagemakerUpdateContextOptions(
[property: CliOption("--context-name")] string ContextName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CliOption("--properties-to-remove")]
    public string[]? PropertiesToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}