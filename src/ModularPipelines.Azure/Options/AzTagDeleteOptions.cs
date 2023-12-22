using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tag", "delete")]
public record AzTagDeleteOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}