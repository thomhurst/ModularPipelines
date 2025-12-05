using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-notebook-instance")]
public record AwsSagemakerDeleteNotebookInstanceOptions(
[property: CliOption("--notebook-instance-name")] string NotebookInstanceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}