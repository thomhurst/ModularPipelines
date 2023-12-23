using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-review-template-lens-review")]
public record AwsWellarchitectedUpdateReviewTemplateLensReviewOptions(
[property: CommandSwitch("--template-arn")] string TemplateArn,
[property: CommandSwitch("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CommandSwitch("--lens-notes")]
    public string? LensNotes { get; set; }

    [CommandSwitch("--pillar-notes")]
    public IEnumerable<KeyValue>? PillarNotes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}