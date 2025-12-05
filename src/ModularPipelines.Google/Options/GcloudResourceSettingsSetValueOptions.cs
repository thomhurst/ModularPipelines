using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-settings", "set-value")]
public record GcloudResourceSettingsSetValueOptions(
[property: CliOption("--value-file")] string ValueFile
) : GcloudOptions;