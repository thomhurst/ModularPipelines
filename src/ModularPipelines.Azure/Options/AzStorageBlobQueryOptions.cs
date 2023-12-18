using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "query")]
public record AzStorageBlobQueryOptions(
[property: CommandSwitch("--query-expression")] string QueryExpression
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--blob-url")]
    public string? BlobUrl { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CommandSwitch("--in-column-separator")]
    public string? InColumnSeparator { get; set; }

    [CommandSwitch("--in-escape-char")]
    public string? InEscapeChar { get; set; }

    [BooleanCommandSwitch("--in-has-header")]
    public bool? InHasHeader { get; set; }

    [CommandSwitch("--in-line-separator")]
    public string? InLineSeparator { get; set; }

    [CommandSwitch("--in-quote-char")]
    public string? InQuoteChar { get; set; }

    [CommandSwitch("--in-record-separator")]
    public string? InRecordSeparator { get; set; }

    [CommandSwitch("--input-format")]
    public string? InputFormat { get; set; }

    [CommandSwitch("--lease-id")]
    public string? LeaseId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--out-column-separator")]
    public string? OutColumnSeparator { get; set; }

    [CommandSwitch("--out-escape-char")]
    public string? OutEscapeChar { get; set; }

    [BooleanCommandSwitch("--out-has-header")]
    public bool? OutHasHeader { get; set; }

    [CommandSwitch("--out-line-separator")]
    public string? OutLineSeparator { get; set; }

    [CommandSwitch("--out-quote-char")]
    public string? OutQuoteChar { get; set; }

    [CommandSwitch("--out-record-separator")]
    public string? OutRecordSeparator { get; set; }

    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--result-file")]
    public string? ResultFile { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--tags-condition")]
    public string? TagsCondition { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}