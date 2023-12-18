using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logic", "integration-account", "update")]
public record AzLogicIntegrationAccountUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--integration-service-environment")]
    public string? IntegrationServiceEnvironment { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}