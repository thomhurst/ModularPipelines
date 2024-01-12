using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "list")]
public record GcloudAssuredWorkloadsListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;