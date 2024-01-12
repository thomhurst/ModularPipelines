using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "create-app-engine-task")]
public record GcloudTasksCreateAppEngineTaskOptions(
[property: PositionalArgument] string TaskId,
[property: CommandSwitch("--queue")] string Queue
) : GcloudOptions
{
    [CommandSwitch("--header")]
    public string? Header { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--method")]
    public string? Method { get; set; }

    [CommandSwitch("--relative-uri")]
    public string? RelativeUri { get; set; }

    [CommandSwitch("--routing")]
    public string[]? Routing { get; set; }

    [CommandSwitch("--schedule-time")]
    public string? ScheduleTime { get; set; }

    [CommandSwitch("--body-content")]
    public string? BodyContent { get; set; }

    [CommandSwitch("--body-file")]
    public string? BodyFile { get; set; }
}