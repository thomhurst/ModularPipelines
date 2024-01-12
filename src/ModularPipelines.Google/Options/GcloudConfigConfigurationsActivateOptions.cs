using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "configurations", "activate")]
public record GcloudConfigConfigurationsActivateOptions(
[property: PositionalArgument] string ConfigurationName
) : GcloudOptions;