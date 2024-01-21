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
    public string? Configuration { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public bool? Nologo { get; set; }

    [CommandSwitch("--output")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--tl")]
    public bool? Tl { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}
