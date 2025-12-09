using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetCleanOptions : DotNetOptions
{
    public DotNetCleanOptions(
        string projectSolution
    )
    {
        CommandParts = ["clean", "[<PROJECT>|<SOLUTION>]"];

        ProjectSolution = projectSolution;
    }

    public DotNetCleanOptions()
    {
        CommandParts = ["clean", "[<PROJECT>|<SOLUTION>]"];
    }

    [CliArgument(Name = "[<PROJECT>|<SOLUTION>]")]
    public virtual string? ProjectSolution { get; set; }

    [CliOption("--configuration")]
    public virtual string? Configuration { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--nologo")]
    public virtual bool? Nologo { get; set; }

    [CliOption("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CliOption("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CliFlag("--tl")]
    public virtual bool? Tl { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
