using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "configurations", "rename")]
public record GcloudConfigConfigurationsRenameOptions(
[property: PositionalArgument] string ConfigurationName,
[property: CommandSwitch("--new-name")] string NewName
) : GcloudOptions;