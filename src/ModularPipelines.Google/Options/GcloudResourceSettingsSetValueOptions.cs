using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-settings", "set-value")]
public record GcloudResourceSettingsSetValueOptions(
[property: CommandSwitch("--value-file")] string ValueFile
) : GcloudOptions;