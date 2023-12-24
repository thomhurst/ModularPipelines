using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "create-exclusions-preview")]
public record AwsInspectorCreateExclusionsPreviewOptions(
[property: CommandSwitch("--assessment-template-arn")] string AssessmentTemplateArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}