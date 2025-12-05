using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "create", "app-engine")]
public record GcloudSchedulerJobsCreateAppEngineOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location,
[property: CliOption("--schedule")] string Schedule
) : GcloudOptions
{
    [CliOption("--attempt-deadline")]
    public string? AttemptDeadline { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CliOption("--http-method")]
    public string? HttpMethod { get; set; }

    [CliOption("--max-backoff")]
    public string? MaxBackoff { get; set; }

    [CliOption("--max-doublings")]
    public string? MaxDoublings { get; set; }

    [CliOption("--max-retry-attempts")]
    public string? MaxRetryAttempts { get; set; }

    [CliOption("--max-retry-duration")]
    public string? MaxRetryDuration { get; set; }

    [CliOption("--min-backoff")]
    public string? MinBackoff { get; set; }

    [CliOption("--relative-url")]
    public string? RelativeUrl { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliOption("--message-body")]
    public string? MessageBody { get; set; }

    [CliOption("--message-body-from-file")]
    public string? MessageBodyFromFile { get; set; }
}