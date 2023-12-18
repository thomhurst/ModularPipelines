using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tag", "delete")]
public record AzTagDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--value")] string Value
) : AzOptions
{
    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

