using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "ssh-keys", "remove")]
public record GcloudBmsSshKeysRemoveOptions(
[property: CliArgument] string SshKey
) : GcloudOptions;