using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "operations", "list")]
public record GcloudAssuredOperationsListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;