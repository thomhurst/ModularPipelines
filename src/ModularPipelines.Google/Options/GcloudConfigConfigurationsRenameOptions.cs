using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "configurations", "rename")]
public record GcloudConfigConfigurationsRenameOptions(
[property: CliArgument] string ConfigurationName,
[property: CliOption("--new-name")] string NewName
) : GcloudOptions;