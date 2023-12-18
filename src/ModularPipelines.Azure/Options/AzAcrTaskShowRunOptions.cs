using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "show-run")]
public record AzAcrTaskShowRunOptions(
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--run-id")] string RunId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}