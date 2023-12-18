using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "node", "file", "download")]
public record AzBatchNodeFileDownloadOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--node-id")] string NodeId,
[property: CommandSwitch("--pool-id")] string PoolId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--end-range")]
    public string? EndRange { get; set; }

    [CommandSwitch("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CommandSwitch("--start-range")]
    public string? StartRange { get; set; }
}

