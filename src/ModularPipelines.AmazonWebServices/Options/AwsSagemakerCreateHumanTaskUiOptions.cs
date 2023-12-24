using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-human-task-ui")]
public record AwsSagemakerCreateHumanTaskUiOptions(
[property: CommandSwitch("--human-task-ui-name")] string HumanTaskUiName,
[property: CommandSwitch("--ui-template")] string UiTemplate
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}