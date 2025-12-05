using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "run", "update")]
public record AzMlRunUpdateOptions(
[property: CliOption("--run")] string Run
) : AzOptions
{
    [CliOption("--add-tag")]
    public string? AddTag { get; set; }

    [CliOption("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}