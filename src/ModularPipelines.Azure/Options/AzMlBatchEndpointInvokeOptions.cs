using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "batch-endpoint", "invoke")]
public record AzMlBatchEndpointInvokeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CliOption("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--input")]
    public string? Input { get; set; }

    [CliOption("--input-type")]
    public string? InputType { get; set; }

    [CliOption("--inputs")]
    public string? Inputs { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--mini-batch-size")]
    public string? MiniBatchSize { get; set; }

    [CliOption("--output-path")]
    public string? OutputPath { get; set; }

    [CliOption("--outputs")]
    public string? Outputs { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}