using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "deployments", "update")]
public record GcloudDeploymentManagerDeploymentsUpdateOptions(
[property: CliArgument] string DeploymentName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--create-policy")]
    public string? CreatePolicy { get; set; }

    [CliOption("--delete-policy")]
    public string? DeletePolicy { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--fingerprint")]
    public string? Fingerprint { get; set; }

    [CliFlag("--preview")]
    public bool? Preview { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--composite-type")]
    public string? CompositeType { get; set; }

    [CliOption("--config")]
    public string? Config { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}