using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "configurations", "delete")]
public record GcloudConfigConfigurationsDeleteOptions(
[property: CliArgument] string ConfigurationNames
) : GcloudOptions;