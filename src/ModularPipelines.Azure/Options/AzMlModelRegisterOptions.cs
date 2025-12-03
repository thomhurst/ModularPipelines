using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "model", "register")]
public record AzMlModelRegisterOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--asset-path")]
    public string? AssetPath { get; set; }

    [CliOption("--cc")]
    public string? Cc { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CliOption("--gb")]
    public string? Gb { get; set; }

    [CliOption("--gc")]
    public string? Gc { get; set; }

    [CliOption("--model-framework")]
    public string? ModelFramework { get; set; }

    [CliOption("--model-framework-version")]
    public string? ModelFrameworkVersion { get; set; }

    [CliOption("--model-path")]
    public string? ModelPath { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--property")]
    public string? Property { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--run-metadata-file")]
    public string? RunMetadataFile { get; set; }

    [CliOption("--sample-input-dataset-id")]
    public string? SampleInputDatasetId { get; set; }

    [CliOption("--sample-output-dataset-id")]
    public string? SampleOutputDatasetId { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("-v")]
    public string? V { get; set; }
}