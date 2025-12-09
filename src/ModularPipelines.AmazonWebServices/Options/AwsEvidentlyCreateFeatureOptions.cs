using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "create-feature")]
public record AwsEvidentlyCreateFeatureOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--project")] string Project,
[property: CliOption("--variations")] string[] Variations
) : AwsOptions
{
    [CliOption("--default-variation")]
    public string? DefaultVariation { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--entity-overrides")]
    public IEnumerable<KeyValue>? EntityOverrides { get; set; }

    [CliOption("--evaluation-strategy")]
    public string? EvaluationStrategy { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}