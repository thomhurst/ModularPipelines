using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "create-feature")]
public record AwsEvidentlyCreateFeatureOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project")] string Project,
[property: CommandSwitch("--variations")] string[] Variations
) : AwsOptions
{
    [CommandSwitch("--default-variation")]
    public string? DefaultVariation { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--entity-overrides")]
    public IEnumerable<KeyValue>? EntityOverrides { get; set; }

    [CommandSwitch("--evaluation-strategy")]
    public string? EvaluationStrategy { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}