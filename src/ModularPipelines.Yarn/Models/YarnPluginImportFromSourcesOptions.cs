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
    public virtual string? Path { get; set; }

    [CommandSwitch("--repository")]
    public virtual string? Repository { get; set; }

    [CommandSwitch("--branch")]
    public virtual string? Branch { get; set; }

    [BooleanCommandSwitch("--no-minify")]
    public virtual bool? NoMinify { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }
}