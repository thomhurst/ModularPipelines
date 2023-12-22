using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("plugin", "import", "from", "sources")]
public record YarnPluginImportFromSourcesOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Name
) : YarnOptions
{
    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [BooleanCommandSwitch("--no-minify")]
    public bool? NoMinify { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}