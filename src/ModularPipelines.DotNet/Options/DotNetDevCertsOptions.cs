using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("dev-certs", "https")]
[ExcludeFromCodeCoverage]
public record DotNetDevCertsOptions : DotNetOptions
{
    [BooleanCommandSwitch("--check")]
    public virtual bool? Check { get; set; }

    [BooleanCommandSwitch("--clean")]
    public virtual bool? Clean { get; set; }

    [CommandSwitch("--export-path")]
    public virtual string? ExportPath { get; set; }

    [BooleanCommandSwitch("--format")]
    public virtual bool? Format { get; set; }

    [BooleanCommandSwitch("--import")]
    public virtual bool? Import { get; set; }

    [BooleanCommandSwitch("--no-password")]
    public virtual bool? NoPassword { get; set; }

    [BooleanCommandSwitch("--password")]
    public virtual bool? Password { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--trust")]
    public virtual bool? Trust { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--version")]
    public virtual bool? Version { get; set; }
}
