using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "start-notebook-instance")]
public record AwsSagemakerStartNotebookInstanceOptions(
[property: CliOption("--notebook-instance-name")] string NotebookInstanceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}