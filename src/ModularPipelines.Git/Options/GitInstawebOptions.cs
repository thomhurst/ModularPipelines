using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("instaweb")]
[ExcludeFromCodeCoverage]
public record GitInstawebOptions : GitOptions
{
    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("--httpd")]
    public bool? Httpd { get; set; }

    [BooleanCommandSwitch("--module-path")]
    public bool? ModulePath { get; set; }

    [BooleanCommandSwitch("--port")]
    public bool? Port { get; set; }

    [BooleanCommandSwitch("--browser")]
    public bool? Browser { get; set; }

    [BooleanCommandSwitch("--start")]
    public bool? Start { get; set; }

    [BooleanCommandSwitch("--stop")]
    public bool? Stop { get; set; }

    [BooleanCommandSwitch("--restart")]
    public bool? Restart { get; set; }
}