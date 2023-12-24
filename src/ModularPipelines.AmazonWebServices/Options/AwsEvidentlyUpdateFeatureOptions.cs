using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "update-feature")]
public record AwsEvidentlyUpdateFeatureOptions(
[property: CommandSwitch("--feature")] string Feature,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--add-or-update-variations")]
    public string[]? AddOrUpdateVariations { get; set; }

    [CommandSwitch("--default-variation")]
    public string? DefaultVariation { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--entity-overrides")]
    public IEnumerable<KeyValue>? EntityOverrides { get; set; }

    [CommandSwitch("--evaluation-strategy")]
    public string? EvaluationStrategy { get; set; }

    [CommandSwitch("--remove-variations")]
    public string[]? RemoveVariations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}