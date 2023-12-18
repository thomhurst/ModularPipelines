using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "lock", "list")]
public record AzAccountLockListOptions : AzOptions
{
    [CommandSwitch("--filter-string")]
    public string? FilterString { get; set; }
}

