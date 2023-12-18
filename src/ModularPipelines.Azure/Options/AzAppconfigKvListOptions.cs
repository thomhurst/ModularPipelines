using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "kv", "list")]
public record AzAppconfigKvListOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--datetime")]
    public string? Datetime { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--fields")]
    public string? Fields { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--resolve-keyvault")]
    public bool? ResolveKeyvault { get; set; }

    [CommandSwitch("--snapshot")]
    public string? Snapshot { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}