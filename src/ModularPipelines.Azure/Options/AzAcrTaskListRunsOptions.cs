using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "list-runs")]
public record AzAcrTaskListRunsOptions(
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-status")]
    public string? RunStatus { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}