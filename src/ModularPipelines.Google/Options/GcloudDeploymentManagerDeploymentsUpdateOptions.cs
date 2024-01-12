using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "deployments", "update")]
public record GcloudDeploymentManagerDeploymentsUpdateOptions(
[property: PositionalArgument] string DeploymentName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--create-policy")]
    public string? CreatePolicy { get; set; }

    [CommandSwitch("--delete-policy")]
    public string? DeletePolicy { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--fingerprint")]
    public string? Fingerprint { get; set; }

    [BooleanCommandSwitch("--preview")]
    public bool? Preview { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--composite-type")]
    public string? CompositeType { get; set; }

    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }
}