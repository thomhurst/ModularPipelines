using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

public record DotNetCleanOptions : DotNetOptions
{
    [CommandSwitch( "c" )]
    public Configuration? Configuration { get; init; }

    [CommandSwitch( "f" )]
    public string? Framework { get; init; }

    [CommandSwitch( "o" )]
    public string? Output { get; init; }

    [BooleanCommandSwitch( "nologo" )]
    public bool? NoLogo { get; init; }

    [BooleanCommandSwitch( "use-current-runtime" )]
    public bool? UseCurrentRuntime { get; init; }
}
