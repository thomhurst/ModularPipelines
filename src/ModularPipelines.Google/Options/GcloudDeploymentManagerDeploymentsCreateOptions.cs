using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "deployments", "create")]
public record GcloudDeploymentManagerDeploymentsCreateOptions(
[property: PositionalArgument] string DeploymentName,
[property: CommandSwitch("--composite-type")] string CompositeType,
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--template")] string Template
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--preview")]
    public bool? Preview { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--automatic-rollback-on-error")]
    public bool? AutomaticRollbackOnError { get; set; }
}