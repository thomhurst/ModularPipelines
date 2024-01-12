using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-login", "ssh-keys", "list")]
public record GcloudComputeOsLoginSshKeysListOptions : GcloudOptions;