using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "configurations", "delete")]
public record GcloudConfigConfigurationsDeleteOptions(
[property: PositionalArgument] string ConfigurationNames
) : GcloudOptions;