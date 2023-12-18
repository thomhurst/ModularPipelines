using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "exec")]
public record AzContainerappExecOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--replica")]
    public string? Replica { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }
}