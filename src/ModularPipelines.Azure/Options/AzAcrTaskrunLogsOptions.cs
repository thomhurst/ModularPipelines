using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "taskrun", "logs")]
public record AzAcrTaskrunLogsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [BooleanCommandSwitch("--no-format")]
    public bool? NoFormat { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}