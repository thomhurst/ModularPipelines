using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "configuration", "create")]
public record AzIotHubConfigurationCreateOptions(
[property: CommandSwitch("--config-id")] string ConfigId,
[property: CommandSwitch("--content")] string Content
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

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--metrics")]
    public string? Metrics { get; set; }

    [CommandSwitch("--pri")]
    public string? Pri { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--target-condition")]
    public string? TargetCondition { get; set; }
}

