using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "batch-endpoint", "invoke")]
public record AzMlBatchEndpointInvokeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CommandSwitch("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [CommandSwitch("--input-type")]
    public string? InputType { get; set; }

    [CommandSwitch("--inputs")]
    public string? Inputs { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--mini-batch-size")]
    public string? MiniBatchSize { get; set; }

    [CommandSwitch("--output-path")]
    public string? OutputPath { get; set; }

    [CommandSwitch("--outputs")]
    public string? Outputs { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}