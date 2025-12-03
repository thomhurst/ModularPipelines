using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "nfs-shares", "rename")]
public record GcloudBmsNfsSharesRenameOptions(
[property: CliArgument] string NfsShare,
[property: CliArgument] string Region,
[property: CliOption("--new-name")] string NewName
) : GcloudOptions;