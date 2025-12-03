using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "update", "pubsub")]
public record GcloudSchedulerJobsUpdatePubsubOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--topic")]
    public string? Topic { get; set; }

    [CliFlag("--clear-attributes")]
    public bool? ClearAttributes { get; set; }

    [CliOption("--remove-attributes")]
    public string[]? RemoveAttributes { get; set; }

    [CliOption("--update-attributes")]
    public IEnumerable<KeyValue>? UpdateAttributes { get; set; }

    [CliFlag("--clear-max-backoff")]
    public bool? ClearMaxBackoff { get; set; }

    [CliOption("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [CliFlag("--clear-max-doublings")]
    public bool? ClearMaxDoublings { get; set; }

    [CliOption("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [CliFlag("--clear-max-retry-attempts")]
    public bool? ClearMaxRetryAttempts { get; set; }

    [CliOption("--max-retry-attempts")]
    public string? MaxRetryAttempts { get; set; }

    [CliFlag("--clear-max-retry-duration")]
    public bool? ClearMaxRetryDuration { get; set; }

    [CliOption("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [CliFlag("--clear-min-backoff")]
    public bool? ClearMinBackoff { get; set; }

    [CliOption("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CliFlag("--clear-time-zone")]
    public bool? ClearTimeZone { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--message-body")]
    public string? MessageBody { get; set; }

    [CliOption("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }
}