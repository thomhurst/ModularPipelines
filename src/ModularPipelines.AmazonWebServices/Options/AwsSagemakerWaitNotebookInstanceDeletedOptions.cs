using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "wait", "notebook-instance-deleted")]
public record AwsSagemakerWaitNotebookInstanceDeletedOptions(
[property: CommandSwitch("--notebook-instance-name")] string NotebookInstanceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}