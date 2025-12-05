using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-transform-job")]
public record AwsSagemakerCreateTransformJobOptions(
[property: CliOption("--transform-job-name")] string TransformJobName,
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--transform-input")] string TransformInput,
[property: CliOption("--transform-output")] string TransformOutput,
[property: CliOption("--transform-resources")] string TransformResources
) : AwsOptions
{
    [CliOption("--max-concurrent-transforms")]
    public int? MaxConcurrentTransforms { get; set; }

    [CliOption("--model-client-config")]
    public string? ModelClientConfig { get; set; }

    [CliOption("--max-payload-in-mb")]
    public int? MaxPayloadInMb { get; set; }

    [CliOption("--batch-strategy")]
    public string? BatchStrategy { get; set; }

    [CliOption("--environment")]
    public IEnumerable<KeyValue>? Environment { get; set; }

    [CliOption("--data-capture-config")]
    public string? DataCaptureConfig { get; set; }

    [CliOption("--data-processing")]
    public string? DataProcessing { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--experiment-config")]
    public string? ExperimentConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}