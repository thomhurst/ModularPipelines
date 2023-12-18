using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logic", "integration-account", "map", "create")]
public record AzLogicIntegrationAccountMapCreateOptions(
[property: CommandSwitch("--integration-account")] int IntegrationAccount,
[property: CommandSwitch("--map-name")] string MapName,
[property: CommandSwitch("--map-type")] string MapType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--map-content")]
    public string? MapContent { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}