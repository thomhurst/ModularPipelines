using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("instaweb")]
[ExcludeFromCodeCoverage]
public record GitInstawebOptions : GitOptions
{
    [BooleanCommandSwitch("--local")]
    public virtual bool? Local { get; set; }

    [BooleanCommandSwitch("--httpd")]
    public virtual bool? Httpd { get; set; }

    [BooleanCommandSwitch("--module-path")]
    public virtual bool? ModulePath { get; set; }

    [BooleanCommandSwitch("--port")]
    public virtual bool? Port { get; set; }

    [BooleanCommandSwitch("--browser")]
    public virtual bool? Browser { get; set; }

    [BooleanCommandSwitch("--start")]
    public virtual bool? Start { get; set; }

    [BooleanCommandSwitch("--stop")]
    public virtual bool? Stop { get; set; }

    [BooleanCommandSwitch("--restart")]
    public virtual bool? Restart { get; set; }
}