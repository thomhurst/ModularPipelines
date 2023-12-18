using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("group", "lock", "create")]
public record AzGroupLockCreateOptions(
[property: CommandSwitch("--lock-type")] string LockType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--notes")]
    public string? Notes { get; set; }
}