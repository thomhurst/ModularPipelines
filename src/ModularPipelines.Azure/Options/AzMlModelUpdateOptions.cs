using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "update")]
public record AzMlModelUpdateOptions(
[property: CommandSwitch("--model-id")] string ModelId
) : AzOptions
{
    [CommandSwitch("--add-property")]
    public string? AddProperty { get; set; }

    [CommandSwitch("--add-tag")]
    public string? AddTag { get; set; }

    [CommandSwitch("--cc")]
    public string? Cc { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--gb")]
    public string? Gb { get; set; }

    [CommandSwitch("--gc")]
    public string? Gc { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--remove-tag")]
    public string? RemoveTag { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sample-input-dataset-id")]
    public string? SampleInputDatasetId { get; set; }

    [CommandSwitch("--sample-output-dataset-id")]
    public string? SampleOutputDatasetId { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}