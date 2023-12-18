using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "dapr-component", "resiliency", "update")]
public record AzContainerappEnvDaprComponentResiliencyUpdateOptions(
[property: CommandSwitch("--dapr-component-name")] string DaprComponentName,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--in-http-delay")]
    public string? InHttpDelay { get; set; }

    [CommandSwitch("--in-http-interval")]
    public string? InHttpInterval { get; set; }

    [CommandSwitch("--in-http-retries")]
    public string? InHttpRetries { get; set; }

    [CommandSwitch("--in-timeout")]
    public string? InTimeout { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--out-http-delay")]
    public string? OutHttpDelay { get; set; }

    [CommandSwitch("--out-http-interval")]
    public string? OutHttpInterval { get; set; }

    [CommandSwitch("--out-http-retries")]
    public string? OutHttpRetries { get; set; }

    [CommandSwitch("--out-timeout")]
    public string? OutTimeout { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--yaml")]
    public string? Yaml { get; set; }
}