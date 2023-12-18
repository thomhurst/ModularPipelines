using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "share", "delete")]
public record AzStorageShareDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--delete-snapshots")]
    public string? DeleteSnapshots { get; set; }

    [BooleanCommandSwitch("--fail-not-exist")]
    public bool? FailNotExist { get; set; }

    [CommandSwitch("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--snapshot")]
    public string? Snapshot { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}