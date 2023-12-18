using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "identity", "assign")]
public record AzAcrTaskIdentityAssignOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--identities")]
    public string? Identities { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}