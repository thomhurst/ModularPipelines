using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "ssh-keys", "add")]
public record GcloudBmsSshKeysAddOptions(
[property: CliArgument] string SshKey,
[property: CliOption("--key")] string Key,
[property: CliOption("--key-file")] string KeyFile
) : GcloudOptions;