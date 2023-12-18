using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "list")]
public record AzStaticwebappListOptions(
[property: CommandSwitch("--branch")] string Branch,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

