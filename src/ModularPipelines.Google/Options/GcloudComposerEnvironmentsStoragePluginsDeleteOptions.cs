using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "storage", "plugins", "delete")]
public record GcloudComposerEnvironmentsStoragePluginsDeleteOptions(
[property: PositionalArgument] string Target,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;