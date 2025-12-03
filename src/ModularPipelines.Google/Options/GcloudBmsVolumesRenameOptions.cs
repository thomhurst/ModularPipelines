using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "rename")]
public record GcloudBmsVolumesRenameOptions(
[property: CliArgument] string Volume,
[property: CliArgument] string Region,
[property: CliOption("--new-name")] string NewName
) : GcloudOptions;