using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetListPackageOptions : DotNetOptions
{
    public DotNetListPackageOptions(
        string projectSolution
    )
    {
        CommandParts = ["list", "[<PROJECT>|<SOLUTION>]", "package"];

        ProjectSolution = projectSolution;
    }

    public DotNetListPackageOptions()
    {
        CommandParts = ["list", "[<PROJECT>|<SOLUTION>]", "package"];
    }

    [CliArgument(Name = "[<PROJECT>|<SOLUTION>]")]
    public virtual string? ProjectSolution { get; set; }

    [CliOption("--config")]
    public virtual string? Config { get; set; }

    [CliFlag("--deprecated")]
    public virtual bool? Deprecated { get; set; }

    [CliOption("--framework")]
    public virtual IEnumerable<string>? Framework { get; set; }

    [CliFlag("--highest-minor")]
    public virtual bool? HighestMinor { get; set; }

    [CliFlag("--highest-patch")]
    public virtual bool? HighestPatch { get; set; }

    [CliFlag("--include-prerelease")]
    public virtual bool? IncludePrerelease { get; set; }

    [CliFlag("--include-transitive")]
    public virtual bool? IncludeTransitive { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--outdated")]
    public virtual bool? Outdated { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliFlag("--vulnerable")]
    public virtual bool? Vulnerable { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--output-version")]
    public virtual string? OutputVersion { get; set; }
}
