using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("instaweb")]
[ExcludeFromCodeCoverage]
public record GitInstawebOptions : GitOptions
{
    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliFlag("--httpd")]
    public virtual bool? Httpd { get; set; }

    [CliFlag("--module-path")]
    public virtual bool? ModulePath { get; set; }

    [CliFlag("--port")]
    public virtual bool? Port { get; set; }

    [CliFlag("--browser")]
    public virtual bool? Browser { get; set; }

    [CliFlag("--start")]
    public virtual bool? Start { get; set; }

    [CliFlag("--stop")]
    public virtual bool? Stop { get; set; }

    [CliFlag("--restart")]
    public virtual bool? Restart { get; set; }
}