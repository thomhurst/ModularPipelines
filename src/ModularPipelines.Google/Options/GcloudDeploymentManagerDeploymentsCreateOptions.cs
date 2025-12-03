using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "deployments", "create")]
public record GcloudDeploymentManagerDeploymentsCreateOptions(
[property: CliArgument] string DeploymentName,
[property: CliOption("--composite-type")] string CompositeType,
[property: CliOption("--config")] string Config,
[property: CliOption("--template")] string Template
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--preview")]
    public bool? Preview { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--automatic-rollback-on-error")]
    public bool? AutomaticRollbackOnError { get; set; }
}