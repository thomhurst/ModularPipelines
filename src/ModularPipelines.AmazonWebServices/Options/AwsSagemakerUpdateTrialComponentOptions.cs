using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-trial-component")]
public record AwsSagemakerUpdateTrialComponentOptions(
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

    [CommandSwitch("--parameters-to-remove")]
    public string[]? ParametersToRemove { get; set; }

    [CommandSwitch("--input-artifacts")]
    public IEnumerable<KeyValue>? InputArtifacts { get; set; }

    [CommandSwitch("--input-artifacts-to-remove")]
    public string[]? InputArtifactsToRemove { get; set; }

    [CommandSwitch("--output-artifacts")]
    public IEnumerable<KeyValue>? OutputArtifacts { get; set; }

    [CommandSwitch("--output-artifacts-to-remove")]
    public string[]? OutputArtifactsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}