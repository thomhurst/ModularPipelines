using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-trial-component")]
public record AwsSagemakerUpdateTrialComponentOptions(
[property: CliOption("--trial-component-name")] string TrialComponentName
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--parameters-to-remove")]
    public string[]? ParametersToRemove { get; set; }

    [CliOption("--input-artifacts")]
    public IEnumerable<KeyValue>? InputArtifacts { get; set; }

    [CliOption("--input-artifacts-to-remove")]
    public string[]? InputArtifactsToRemove { get; set; }

    [CliOption("--output-artifacts")]
    public IEnumerable<KeyValue>? OutputArtifacts { get; set; }

    [CliOption("--output-artifacts-to-remove")]
    public string[]? OutputArtifactsToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}