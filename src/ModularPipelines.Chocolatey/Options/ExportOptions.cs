using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("export")]
public record ExportOptions : ChocoOptions
{
    [CommandSwitch("--output-file-path")]
    public virtual string? OutputFilePath { get; set; }

    [BooleanCommandSwitch("--include-version")]
    public virtual bool? IncludeVersion { get; set; }
}