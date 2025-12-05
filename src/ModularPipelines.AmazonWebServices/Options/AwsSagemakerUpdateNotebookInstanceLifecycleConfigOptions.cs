using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-notebook-instance-lifecycle-config")]
public record AwsSagemakerUpdateNotebookInstanceLifecycleConfigOptions(
[property: CliOption("--notebook-instance-lifecycle-config-name")] string NotebookInstanceLifecycleConfigName
) : AwsOptions
{
    [CliOption("--on-create")]
    public string[]? OnCreate { get; set; }

    [CliOption("--on-start")]
    public string[]? OnStart { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}