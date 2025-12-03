using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "update", "app-engine")]
public record GcloudSchedulerJobsUpdateAppEngineOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--attempt-deadline")]
    public string? AttemptDeadline { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--http-method")]
    public string? HttpMethod { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliFlag("--clear-headers")]
    public bool? ClearHeaders { get; set; }

    [CliOption("--remove-headers")]
    public string[]? RemoveHeaders { get; set; }

    [CliOption("--update-headers")]
    public IEnumerable<KeyValue>? UpdateHeaders { get; set; }

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

    [CliFlag("--clear-message-body")]
    public bool? ClearMessageBody { get; set; }

    [CliOption("--message-body")]
    public string? MessageBody { get; set; }

    [CliOption("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }

    [CliFlag("--clear-min-backoff")]
    public bool? ClearMinBackoff { get; set; }

    [CliOption("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CliFlag("--clear-relative-url")]
    public bool? ClearRelativeUrl { get; set; }

    [CliOption("--relative-url")]
    public string? RelativeUrl { get; set; }

    [CliFlag("--clear-service")]
    public bool? ClearService { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliFlag("--clear-time-zone")]
    public bool? ClearTimeZone { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }
}