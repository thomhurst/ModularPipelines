using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("install")]
public record YarnInstallOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--immutable")]
    public virtual bool? Immutable { get; set; }

    [BooleanCommandSwitch("--immutable-cache")]
    public virtual bool? ImmutableCache { get; set; }

    [BooleanCommandSwitch("--refresh-lockfile")]
    public virtual bool? RefreshLockfile { get; set; }

    [BooleanCommandSwitch("--check-cache")]
    public virtual bool? CheckCache { get; set; }

    [BooleanCommandSwitch("--check-resolutions")]
    public virtual bool? CheckResolutions { get; set; }

    [BooleanCommandSwitch("--inline-builds")]
    public virtual bool? InlineBuilds { get; set; }

    [CommandSwitch("--mode")]
    public virtual string? Mode { get; set; }
}