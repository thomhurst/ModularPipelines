using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("set", "version", "from", "sources")]
public record YarnSetVersionFromSourcesOptions : YarnOptions
{
    [CommandSwitch("--path")]
    public virtual string? Path { get; set; }

    [CommandSwitch("--repository")]
    public virtual string? Repository { get; set; }

    [CommandSwitch("--branch")]
    public virtual string? Branch { get; set; }

    [CommandSwitch("--plugin")]
    public virtual string? Plugin { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--no-minify")]
    public virtual bool? NoMinify { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--skip-plugins")]
    public virtual bool? SkipPlugins { get; set; }
}