using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "lock", "create")]
public record AzAccountLockCreateOptions(
[property: CommandSwitch("--lock-type")] string LockType,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--notes")]
    public string? Notes { get; set; }
}