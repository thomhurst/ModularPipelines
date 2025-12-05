using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-login", "ssh-keys", "remove")]
public record GcloudComputeOsLoginSshKeysRemoveOptions(
[property: CliOption("--key")] string Key,
[property: CliOption("--key-file")] string KeyFile
) : GcloudOptions;