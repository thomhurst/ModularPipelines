using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("pack")]
public record DotNetPackOptions : DotNetOptions
{

    [CommandSwitch("-c")]
    public Configuration? Configuration { get; init; }


    [CommandSwitch("-o")]
    public string? Output { get; init; }


    [CommandSwitch("-s")]
    public string? Source { get; init; }

    [CommandEqualsSeparatorSwitch("--version-suffix", SwitchValueSeparator = " ")]
    public string? VersionSuffix { get; init; }

    [BooleanCommandSwitch("--include-source")]
    public bool? IncludeSource { get; init; }

    [BooleanCommandSwitch("--include-symbols")]
    public bool? IncludeSymbols { get; init; }

    [BooleanCommandSwitch("--serviceable")]
    public bool? Serviceable { get; init; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; init; }

    [BooleanCommandSwitch("--no-dependencies")]
    public bool? NoDependencies { get; init; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; init; }

    [BooleanCommandSwitch("--nologo")]
    public bool? NoLogo { get; init; }
}
