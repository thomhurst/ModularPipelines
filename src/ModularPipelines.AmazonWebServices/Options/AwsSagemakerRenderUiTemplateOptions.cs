using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "render-ui-template")]
public record AwsSagemakerRenderUiTemplateOptions(
[property: CommandSwitch("--task")] string Task,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--ui-template")]
    public string? UiTemplate { get; set; }

    [CommandSwitch("--human-task-ui-arn")]
    public string? HumanTaskUiArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}