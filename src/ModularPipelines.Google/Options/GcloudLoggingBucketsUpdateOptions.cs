using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "buckets", "update")]
public record GcloudLoggingBucketsUpdateOptions(
[property: PositionalArgument] string BucketId,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--add-index")]
    public IEnumerable<KeyValue>? AddIndex { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--clear-indexes")]
    public bool? ClearIndexes { get; set; }

    [CommandSwitch("--cmek-kms-key-name")]
    public string? CmekKmsKeyName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-analytics")]
    public bool? EnableAnalytics { get; set; }

    [BooleanCommandSwitch("--locked")]
    public bool? Locked { get; set; }

    [CommandSwitch("--remove-indexes")]
    public string[]? RemoveIndexes { get; set; }

    [CommandSwitch("--restricted-fields")]
    public string[]? RestrictedFields { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--update-index")]
    public IEnumerable<KeyValue>? UpdateIndex { get; set; }
}