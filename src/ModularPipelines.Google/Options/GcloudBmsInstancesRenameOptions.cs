using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "instances", "rename")]
public record GcloudBmsInstancesRenameOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--new-name")] string NewName
) : GcloudOptions;