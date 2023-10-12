using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("install")]
public record YarnInstallOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--immutable")]
    public bool? Immutable { get; set; }

    [BooleanCommandSwitch("--immutable-cache")]
    public bool? ImmutableCache { get; set; }

    [BooleanCommandSwitch("--refresh-lockfile")]
    public bool? RefreshLockfile { get; set; }

    [BooleanCommandSwitch("--check-cache")]
    public bool? CheckCache { get; set; }

    [BooleanCommandSwitch("--check-resolutions")]
    public bool? CheckResolutions { get; set; }

    [BooleanCommandSwitch("--inline-builds")]
    public bool? InlineBuilds { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }
}