using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "render-ui-template")]
public record AwsSagemakerRenderUiTemplateOptions(
[property: CliOption("--task")] string Task,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--ui-template")]
    public string? UiTemplate { get; set; }

    [CliOption("--human-task-ui-arn")]
    public string? HumanTaskUiArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}