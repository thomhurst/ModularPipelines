using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "buckets", "update")]
public record GcloudLoggingBucketsUpdateOptions(
[property: CliArgument] string BucketId,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--add-index")]
    public IEnumerable<KeyValue>? AddIndex { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--clear-indexes")]
    public bool? ClearIndexes { get; set; }

    [CliOption("--cmek-kms-key-name")]
    public string? CmekKmsKeyName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-analytics")]
    public bool? EnableAnalytics { get; set; }

    [CliFlag("--locked")]
    public bool? Locked { get; set; }

    [CliOption("--remove-indexes")]
    public string[]? RemoveIndexes { get; set; }

    [CliOption("--restricted-fields")]
    public string[]? RestrictedFields { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--update-index")]
    public IEnumerable<KeyValue>? UpdateIndex { get; set; }
}