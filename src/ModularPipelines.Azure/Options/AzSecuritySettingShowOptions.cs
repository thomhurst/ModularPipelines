using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "setting", "show")]
public record AzSecuritySettingShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;