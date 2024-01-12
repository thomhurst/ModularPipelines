using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "levels", "conditions", "list")]
public record GcloudAccessContextManagerLevelsConditionsListOptions(
[property: CommandSwitch("--level")] string Level,
[property: CommandSwitch("--policy")] string Policy
) : GcloudOptions;