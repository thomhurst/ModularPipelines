using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("lint")]
[ExcludeFromCodeCoverage]
public record HelmLintOptions : HelmOptions
{
    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--set")]
    public virtual string[]? Set { get; set; }

    [CliOption("--set-file")]
    public virtual string[]? SetFile { get; set; }

    [CliOption("--set-json")]
    public virtual string[]? SetJson { get; set; }

    [CliOption("--set-literal")]
    public virtual string[]? SetLiteral { get; set; }

    [CliOption("--set-string")]
    public virtual string[]? SetString { get; set; }

    [CliFlag("--strict")]
    public virtual bool? Strict { get; set; }

    [CliOption("--values")]
    public virtual string[]? Values { get; set; }

    [CliFlag("--with-subcharts")]
    public virtual bool? WithSubcharts { get; set; }
}