using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("export")]
public record ExportOptions : ChocoOptions
{
    [CliOption("--output-file-path")]
    public virtual string? OutputFilePath { get; set; }

    [CliFlag("--include-version")]
    public virtual bool? IncludeVersion { get; set; }
}