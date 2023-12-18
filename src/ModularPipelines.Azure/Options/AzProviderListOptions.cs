using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "list")]
public record AzProviderListOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}

