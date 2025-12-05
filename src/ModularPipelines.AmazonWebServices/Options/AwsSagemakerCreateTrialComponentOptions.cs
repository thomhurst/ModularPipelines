using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-trial-component")]
public record AwsSagemakerCreateTrialComponentOptions(
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

    [CliOption("--input-artifacts")]
    public IEnumerable<KeyValue>? InputArtifacts { get; set; }

    [CliOption("--output-artifacts")]
    public IEnumerable<KeyValue>? OutputArtifacts { get; set; }

    [CliOption("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}