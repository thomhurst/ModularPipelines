using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "notifications", "create")]
public record GcloudStorageBucketsNotificationsCreateOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--custom-attributes")]
    public IEnumerable<KeyValue>? CustomAttributes { get; set; }

    [CommandSwitch("--event-types")]
    public string[]? EventTypes { get; set; }

    [CommandSwitch("--object-prefix")]
    public string? ObjectPrefix { get; set; }

    [CommandSwitch("--payload-format")]
    public string? PayloadFormat { get; set; }

    [BooleanCommandSwitch("--skip-topic-setup")]
    public bool? SkipTopicSetup { get; set; }

    [CommandSwitch("--topic")]
    public string? Topic { get; set; }
}