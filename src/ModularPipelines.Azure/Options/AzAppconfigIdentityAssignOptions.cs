using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "identity", "assign")]
public record AzAppconfigIdentityAssignOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--identities")]
    public string? Identities { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}