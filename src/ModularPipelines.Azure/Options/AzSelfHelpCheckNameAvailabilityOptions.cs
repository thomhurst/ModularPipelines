using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "check-name-availability")]
public record AzSelfHelpCheckNameAvailabilityOptions(
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}

