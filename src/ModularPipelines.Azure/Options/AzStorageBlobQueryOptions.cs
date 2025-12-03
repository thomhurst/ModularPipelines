using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob", "query")]
public record AzStorageBlobQueryOptions(
[property: CliOption("--query-expression")] string QueryExpression
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--blob-url")]
    public string? BlobUrl { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CliOption("--in-column-separator")]
    public string? InColumnSeparator { get; set; }

    [CliOption("--in-escape-char")]
    public string? InEscapeChar { get; set; }

    [CliFlag("--in-has-header")]
    public bool? InHasHeader { get; set; }

    [CliOption("--in-line-separator")]
    public string? InLineSeparator { get; set; }

    [CliOption("--in-quote-char")]
    public string? InQuoteChar { get; set; }

    [CliOption("--in-record-separator")]
    public string? InRecordSeparator { get; set; }

    [CliOption("--input-format")]
    public string? InputFormat { get; set; }

    [CliOption("--lease-id")]
    public string? LeaseId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--out-column-separator")]
    public string? OutColumnSeparator { get; set; }

    [CliOption("--out-escape-char")]
    public string? OutEscapeChar { get; set; }

    [CliFlag("--out-has-header")]
    public bool? OutHasHeader { get; set; }

    [CliOption("--out-line-separator")]
    public string? OutLineSeparator { get; set; }

    [CliOption("--out-quote-char")]
    public string? OutQuoteChar { get; set; }

    [CliOption("--out-record-separator")]
    public string? OutRecordSeparator { get; set; }

    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--result-file")]
    public string? ResultFile { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--tags-condition")]
    public string? TagsCondition { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}