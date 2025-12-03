using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "update-feature")]
public record AwsEvidentlyUpdateFeatureOptions(
[property: CliOption("--feature")] string Feature,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--add-or-update-variations")]
    public string[]? AddOrUpdateVariations { get; set; }

    [CliOption("--default-variation")]
    public string? DefaultVariation { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--entity-overrides")]
    public IEnumerable<KeyValue>? EntityOverrides { get; set; }

    [CliOption("--evaluation-strategy")]
    public string? EvaluationStrategy { get; set; }

    [CliOption("--remove-variations")]
    public string[]? RemoveVariations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}