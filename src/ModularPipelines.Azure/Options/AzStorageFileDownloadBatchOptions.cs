using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "download-batch")]
public record AzStorageFileDownloadBatchOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [BooleanCommandSwitch("--dryrun")]
    public bool? Dryrun { get; set; }

    [CommandSwitch("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CommandSwitch("--max-connections")]
    public string? MaxConnections { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [CommandSwitch("--pattern")]
    public string? Pattern { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--snapshot")]
    public string? Snapshot { get; set; }

    [BooleanCommandSwitch("--validate-content")]
    public bool? ValidateContent { get; set; }
}