using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-notebook-instance-lifecycle-config")]
public record AwsSagemakerDeleteNotebookInstanceLifecycleConfigOptions(
[property: CliOption("--notebook-instance-lifecycle-config-name")] string NotebookInstanceLifecycleConfigName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}