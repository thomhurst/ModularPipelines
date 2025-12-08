using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("install")]
public record YarnInstallOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--immutable")]
    public virtual bool? Immutable { get; set; }

    [CliFlag("--immutable-cache")]
    public virtual bool? ImmutableCache { get; set; }

    [CliFlag("--refresh-lockfile")]
    public virtual bool? RefreshLockfile { get; set; }

    [CliFlag("--check-cache")]
    public virtual bool? CheckCache { get; set; }

    [CliFlag("--check-resolutions")]
    public virtual bool? CheckResolutions { get; set; }

    [CliFlag("--inline-builds")]
    public virtual bool? InlineBuilds { get; set; }

    [CliOption("--mode")]
    public virtual string? Mode { get; set; }
}