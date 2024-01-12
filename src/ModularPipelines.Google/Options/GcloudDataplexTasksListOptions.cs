using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "tasks", "list")]
public record GcloudDataplexTasksListOptions(
[property: CommandSwitch("--lake")] string Lake,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;