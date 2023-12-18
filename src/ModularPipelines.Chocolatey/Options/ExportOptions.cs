using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("export")]
public record ExportOptions : ChocoOptions
{
    [CommandSwitch("--output-file-path")]
    public string? OutputFilePath { get; set; }

    [BooleanCommandSwitch("--include-version")]
    public bool? IncludeVersion { get; set; }
}