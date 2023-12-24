using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-transform-job")]
public record AwsSagemakerCreateTransformJobOptions(
[property: CommandSwitch("--transform-job-name")] string TransformJobName,
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--transform-input")] string TransformInput,
[property: CommandSwitch("--transform-output")] string TransformOutput,
[property: CommandSwitch("--transform-resources")] string TransformResources
) : AwsOptions
{
    [CommandSwitch("--max-concurrent-transforms")]
    public int? MaxConcurrentTransforms { get; set; }

    [CommandSwitch("--model-client-config")]
    public string? ModelClientConfig { get; set; }

    [CommandSwitch("--max-payload-in-mb")]
    public int? MaxPayloadInMb { get; set; }

    [CommandSwitch("--batch-strategy")]
    public string? BatchStrategy { get; set; }

    [CommandSwitch("--environment")]
    public IEnumerable<KeyValue>? Environment { get; set; }

    [CommandSwitch("--data-capture-config")]
    public string? DataCaptureConfig { get; set; }

    [CommandSwitch("--data-processing")]
    public string? DataProcessing { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--experiment-config")]
    public string? ExperimentConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}