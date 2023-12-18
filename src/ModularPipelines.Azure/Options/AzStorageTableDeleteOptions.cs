using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "table", "delete")]
public record AzStorageTableDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--fail-not-exist")]
    public bool? FailNotExist { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--table-endpoint")]
    public string? TableEndpoint { get; set; }
}