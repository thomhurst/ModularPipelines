using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "setting", "update")]
public record AzSecuritySettingUpdateOptions(
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--name")] string Name
) : AzOptions;