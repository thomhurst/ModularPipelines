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

    [PositionalArgument(PlaceholderName = "[<PROJECT>|<SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CommandSwitch("--configuration")]
    public virtual string? Configuration { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public virtual bool? Nologo { get; set; }

    [CommandSwitch("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--tl")]
    public virtual bool? Tl { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
