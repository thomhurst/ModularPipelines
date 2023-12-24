using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-notebook-instance-lifecycle-config")]
public record AwsSagemakerCreateNotebookInstanceLifecycleConfigOptions(
[property: CommandSwitch("--notebook-instance-lifecycle-config-name")] string NotebookInstanceLifecycleConfigName
) : AwsOptions
{
    [CommandSwitch("--on-create")]
    public string[]? OnCreate { get; set; }

    [CommandSwitch("--on-start")]
    public string[]? OnStart { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}