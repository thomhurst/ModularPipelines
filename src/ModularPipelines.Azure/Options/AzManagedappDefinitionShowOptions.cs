using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedapp", "definition", "show")]
public record AzManagedappDefinitionShowOptions(
[property: CommandSwitch("--authorizations")] string Authorizations,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--lock-level")] string LockLevel
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}