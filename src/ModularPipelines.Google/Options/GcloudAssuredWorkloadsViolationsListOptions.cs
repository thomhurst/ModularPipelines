using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "violations", "list")]
public record GcloudAssuredWorkloadsViolationsListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--organization")] string Organization,
[property: CommandSwitch("--workload")] string Workload
) : GcloudOptions;