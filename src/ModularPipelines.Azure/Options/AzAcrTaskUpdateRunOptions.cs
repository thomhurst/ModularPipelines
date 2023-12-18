using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "update-run")]
public record AzAcrTaskUpdateRunOptions(
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--run-id")] string RunId
) : AzOptions
{
    [BooleanCommandSwitch("--no-archive")]
    public bool? NoArchive { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

