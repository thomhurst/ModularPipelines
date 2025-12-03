using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("bugreport")]
[ExcludeFromCodeCoverage]
public record GitBugreportOptions : GitOptions
{
    [CliOption("--output-directory", Format = OptionFormat.EqualsSeparated)]
    public string? OutputDirectory { get; set; }

    [CliOption("--suffix", Format = OptionFormat.EqualsSeparated)]
    public string? Suffix { get; set; }

    [CliFlag("--no-diagnose")]
    public virtual bool? NoDiagnose { get; set; }

    [CliOption("--diagnose", Format = OptionFormat.EqualsSeparated)]
    public string? Diagnose { get; set; }
}