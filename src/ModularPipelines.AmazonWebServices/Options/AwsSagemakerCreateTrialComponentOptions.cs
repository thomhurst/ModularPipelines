using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-trial-component")]
public record AwsSagemakerCreateTrialComponentOptions(
[property: CommandSwitch("--trial-component-name")] string TrialComponentName
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--input-artifacts")]
    public IEnumerable<KeyValue>? InputArtifacts { get; set; }

    [CommandSwitch("--output-artifacts")]
    public IEnumerable<KeyValue>? OutputArtifacts { get; set; }

    [CommandSwitch("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}