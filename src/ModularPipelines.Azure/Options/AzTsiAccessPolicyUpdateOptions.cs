using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "access-policy", "update")]
public record AzTsiAccessPolicyUpdateOptions : AzOptions
{
    [CommandSwitch("--access-policy-name")]
    public string? AccessPolicyName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--roles")]
    public string? Roles { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

