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
    public string[]? Set { get; set; }

    [CliOption("--set-file")]
    public string[]? SetFile { get; set; }

    [CliOption("--set-json")]
    public string[]? SetJson { get; set; }

    [CliOption("--set-literal")]
    public string[]? SetLiteral { get; set; }

    [CliOption("--set-string")]
    public string[]? SetString { get; set; }

    [CliFlag("--strict")]
    public virtual bool? Strict { get; set; }

    [CliOption("--values")]
    public string[]? Values { get; set; }

    [CliFlag("--with-subcharts")]
    public virtual bool? WithSubcharts { get; set; }
}