using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("dev-certs", "https")]
[ExcludeFromCodeCoverage]
public record DotNetDevCertsOptions : DotNetOptions
{
    [BooleanCommandSwitch("--check")]
    public bool? Check { get; set; }

    [BooleanCommandSwitch("--clean")]
    public bool? Clean { get; set; }

    [CommandSwitch("--export-path")]
    public string? ExportPath { get; set; }

    [BooleanCommandSwitch("--format")]
    public bool? Format { get; set; }

    [BooleanCommandSwitch("--import")]
    public bool? Import { get; set; }

    [BooleanCommandSwitch("--no-password")]
    public bool? NoPassword { get; set; }

    [BooleanCommandSwitch("--password")]
    public bool? Password { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--trust")]
    public bool? Trust { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--version")]
    public bool? Version { get; set; }
}
