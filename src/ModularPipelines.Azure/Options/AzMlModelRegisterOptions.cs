using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "register")]
public record AzMlModelRegisterOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--asset-path")]
    public string? AssetPath { get; set; }

    [CommandSwitch("--cc")]
    public string? Cc { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CommandSwitch("--gb")]
    public string? Gb { get; set; }

    [CommandSwitch("--gc")]
    public string? Gc { get; set; }

    [CommandSwitch("--model-framework")]
    public string? ModelFramework { get; set; }

    [CommandSwitch("--model-framework-version")]
    public string? ModelFrameworkVersion { get; set; }

    [CommandSwitch("--model-path")]
    public string? ModelPath { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--property")]
    public string? Property { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--run-metadata-file")]
    public string? RunMetadataFile { get; set; }

    [CommandSwitch("--sample-input-dataset-id")]
    public string? SampleInputDatasetId { get; set; }

    [CommandSwitch("--sample-output-dataset-id")]
    public string? SampleOutputDatasetId { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}

