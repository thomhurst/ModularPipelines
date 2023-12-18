using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "api", "definition", "create")]
public record AzApicApiDefinitionCreateOptions(
[property: CommandSwitch("--api")] string Api,
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}