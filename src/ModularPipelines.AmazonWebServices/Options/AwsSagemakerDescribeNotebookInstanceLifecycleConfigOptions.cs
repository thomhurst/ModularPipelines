using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-notebook-instance-lifecycle-config")]
public record AwsSagemakerDescribeNotebookInstanceLifecycleConfigOptions(
[property: CommandSwitch("--notebook-instance-lifecycle-config-name")] string NotebookInstanceLifecycleConfigName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}