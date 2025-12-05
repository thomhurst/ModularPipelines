using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("plugin", "import", "from", "sources")]
public record YarnPluginImportFromSourcesOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Name
) : YarnOptions
{
    [CliOption("--path")]
    public virtual string? Path { get; set; }

    [CliOption("--repository")]
    public virtual string? Repository { get; set; }

    [CliOption("--branch")]
    public virtual string? Branch { get; set; }

    [CliFlag("--no-minify")]
    public virtual bool? NoMinify { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}