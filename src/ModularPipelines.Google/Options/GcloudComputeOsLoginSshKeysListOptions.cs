using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-login", "ssh-keys", "list")]
public record GcloudComputeOsLoginSshKeysListOptions : GcloudOptions;