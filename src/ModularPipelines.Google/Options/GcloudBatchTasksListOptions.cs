using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "tasks", "list")]
public record GcloudBatchTasksListOptions(
[property: CommandSwitch("--job")] string Job,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;