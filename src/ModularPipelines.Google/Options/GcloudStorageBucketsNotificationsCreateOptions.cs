using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "notifications", "create")]
public record GcloudStorageBucketsNotificationsCreateOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--custom-attributes")]
    public IEnumerable<KeyValue>? CustomAttributes { get; set; }

    [CliOption("--event-types")]
    public string[]? EventTypes { get; set; }

    [CliOption("--object-prefix")]
    public string? ObjectPrefix { get; set; }

    [CliOption("--payload-format")]
    public string? PayloadFormat { get; set; }

    [CliFlag("--skip-topic-setup")]
    public bool? SkipTopicSetup { get; set; }

    [CliOption("--topic")]
    public string? Topic { get; set; }
}