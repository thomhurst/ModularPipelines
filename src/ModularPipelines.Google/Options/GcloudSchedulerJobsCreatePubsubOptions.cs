using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "create", "pubsub")]
public record GcloudSchedulerJobsCreatePubsubOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location,
[property: CliOption("--schedule")] string Schedule,
[property: CliOption("--topic")] string Topic,
[property: CliOption("--attributes")] string Attributes,
[property: CliOption("--message-body")] string MessageBody,
[property: CliOption("--message-body-from-file")] string MessageBodyFromFile
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

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

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }
}