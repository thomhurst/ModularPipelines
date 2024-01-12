using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "buckets", "create")]
public record GcloudLoggingBucketsCreateOptions(
[property: PositionalArgument] string BucketId,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cmek-kms-key-name")]
    public string? CmekKmsKeyName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-analytics")]
    public bool? EnableAnalytics { get; set; }

    [CommandSwitch("--index")]
    public IEnumerable<KeyValue>? Index { get; set; }

    [CommandSwitch("--restricted-fields")]
    public string[]? RestrictedFields { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }
}