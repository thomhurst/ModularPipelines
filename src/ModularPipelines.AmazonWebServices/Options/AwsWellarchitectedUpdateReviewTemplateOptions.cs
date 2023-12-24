using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-review-template")]
public record AwsWellarchitectedUpdateReviewTemplateOptions(
[property: CommandSwitch("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CommandSwitch("--template-name")]
    public string? TemplateName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--lenses-to-associate")]
    public string[]? LensesToAssociate { get; set; }

    [CommandSwitch("--lenses-to-disassociate")]
    public string[]? LensesToDisassociate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}