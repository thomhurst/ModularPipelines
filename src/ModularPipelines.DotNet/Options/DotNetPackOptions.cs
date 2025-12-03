using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetPackOptions : DotNetOptions
{
    public DotNetPackOptions(
        string projectSolution
    )
    {
        CommandParts = ["pack", "[<PROJECT>|<SOLUTION>]"];

        ProjectSolution = projectSolution;
    }

    public DotNetPackOptions()
    {
        CommandParts = ["pack", "[<PROJECT>|<SOLUTION>]"];
    }

    [CliArgument(Name = "[<PROJECT>|<SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CliOption("--configuration")]
    public virtual string? Configuration { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--include-source")]
    public virtual bool? IncludeSource { get; set; }

    [CliFlag("--include-symbols")]
    public virtual bool? IncludeSymbols { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [CliFlag("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [CliFlag("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CliFlag("--nologo")]
    public virtual bool? Nologo { get; set; }

    [CliOption("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CliOption("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CliFlag("--serviceable")]
    public virtual bool? Serviceable { get; set; }

    [CliFlag("--tl")]
    public virtual bool? Tl { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliOption("--version-suffix")]
    public virtual string? VersionSuffix { get; set; }
}
