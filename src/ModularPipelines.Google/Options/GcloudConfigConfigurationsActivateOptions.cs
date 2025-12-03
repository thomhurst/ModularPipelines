using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "configurations", "activate")]
public record GcloudConfigConfigurationsActivateOptions(
[property: CliArgument] string ConfigurationName
) : GcloudOptions;