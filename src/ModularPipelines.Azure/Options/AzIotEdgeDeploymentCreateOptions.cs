using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "edge", "deployment", "create")]
public record AzIotEdgeDeploymentCreateOptions(
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--deployment-id")] string DeploymentId
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--cl")]
    public string? Cl { get; set; }

    [CommandSwitch("--cmq")]
    public string? Cmq { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--lab")]
    public string? Lab { get; set; }

    [BooleanCommandSwitch("--layered")]
    public bool? Layered { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--metrics")]
    public string? Metrics { get; set; }

    [BooleanCommandSwitch("--no-validation")]
    public bool? NoValidation { get; set; }

    [CommandSwitch("--pri")]
    public string? Pri { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--target-condition")]
    public string? TargetCondition { get; set; }
}