using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliCommand("dev-certs", "https")]
[ExcludeFromCodeCoverage]
public record DotNetDevCertsOptions : DotNetOptions
{
    [CliFlag("--check")]
    public virtual bool? Check { get; set; }

    [CliFlag("--clean")]
    public virtual bool? Clean { get; set; }

    [CliOption("--export-path")]
    public virtual string? ExportPath { get; set; }

    [CliFlag("--format")]
    public virtual bool? Format { get; set; }

    [CliFlag("--import")]
    public virtual bool? Import { get; set; }

    [CliFlag("--no-password")]
    public virtual bool? NoPassword { get; set; }

    [CliFlag("--password")]
    public virtual bool? Password { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--trust")]
    public virtual bool? Trust { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--version")]
    public virtual bool? Version { get; set; }
}
