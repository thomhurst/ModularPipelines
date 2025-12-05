using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "networks", "rename")]
public record GcloudBmsNetworksRenameOptions(
[property: CliArgument] string Network,
[property: CliArgument] string Region,
[property: CliOption("--new-name")] string NewName
) : GcloudOptions;