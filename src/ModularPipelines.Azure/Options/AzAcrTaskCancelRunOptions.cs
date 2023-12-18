using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "cancel-run")]
public record AzAcrTaskCancelRunOptions(
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--run-id")] string RunId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}