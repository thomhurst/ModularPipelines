using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "create-app-engine-task")]
public record GcloudTasksCreateAppEngineTaskOptions(
[property: CliArgument] string TaskId,
[property: CliOption("--queue")] string Queue
) : GcloudOptions
{
    [CliOption("--header")]
    public string? Header { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--method")]
    public string? Method { get; set; }

    [CliOption("--relative-uri")]
    public string? RelativeUri { get; set; }

    [CliOption("--routing")]
    public string[]? Routing { get; set; }

    [CliOption("--schedule-time")]
    public string? ScheduleTime { get; set; }

    [CliOption("--body-content")]
    public string? BodyContent { get; set; }

    [CliOption("--body-file")]
    public string? BodyFile { get; set; }
}