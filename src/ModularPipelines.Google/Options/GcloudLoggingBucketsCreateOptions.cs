using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "buckets", "create")]
public record GcloudLoggingBucketsCreateOptions(
[property: CliArgument] string BucketId,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cmek-kms-key-name")]
    public string? CmekKmsKeyName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-analytics")]
    public bool? EnableAnalytics { get; set; }

    [CliOption("--index")]
    public IEnumerable<KeyValue>? Index { get; set; }

    [CliOption("--restricted-fields")]
    public string[]? RestrictedFields { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }
}