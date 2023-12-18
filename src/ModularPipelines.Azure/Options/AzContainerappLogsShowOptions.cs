using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "logs", "show")]
public record AzContainerappLogsShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--replica")]
    public string? Replica { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--tail")]
    public string? Tail { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}

