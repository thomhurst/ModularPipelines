using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "lock", "list")]
public record AzAccountLockListOptions : AzOptions
{
    [CommandSwitch("--filter-string")]
    public string? FilterString { get; set; }
}