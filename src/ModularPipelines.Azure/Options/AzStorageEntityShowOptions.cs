using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "entity", "show")]
public record AzStorageEntityShowOptions(
[property: CommandSwitch("--partition-key")] string PartitionKey,
[property: CommandSwitch("--row-key")] string RowKey,
[property: CommandSwitch("--table-name")] string TableName
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

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--select")]
    public string? Select { get; set; }

    [CommandSwitch("--table-endpoint")]
    public string? TableEndpoint { get; set; }
}