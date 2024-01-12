using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "tasks", "jobs", "list")]
public record GcloudDataplexTasksJobsListOptions(
[property: CommandSwitch("--task")] string Task,
[property: CommandSwitch("--lake")] string Lake,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;