using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "setting", "update")]
public record AzSecuritySettingUpdateOptions(
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}