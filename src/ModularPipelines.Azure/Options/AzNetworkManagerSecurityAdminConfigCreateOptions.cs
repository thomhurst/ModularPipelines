using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "security-admin-config", "create")]
public record AzNetworkManagerSecurityAdminConfigCreateOptions(
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--apply-on")]
    public string? ApplyOn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}

