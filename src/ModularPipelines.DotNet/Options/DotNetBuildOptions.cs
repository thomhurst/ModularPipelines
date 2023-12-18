using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("build")]
[ExcludeFromCodeCoverage]
public record DotNetBuildOptions : DotNetOptions
{
    [CommandSwitch("-c")]
    public Configuration? Configuration { get; init; } = Options.Configuration.Release;

    [CommandSwitch("-f")]
    public string? Framework { get; init; }

    [CommandSwitch("-a")]
    public string? Architecture { get; init; }

    [CommandSwitch("-o")]
    public string? Output { get; init; }

    [CommandSwitch("-s")]
    public string? Source { get; init; }

    [CommandEqualsSeparatorSwitch("--os", SwitchValueSeparator = " ")]
    public string? OperatingSystem { get; init; }

    [CommandEqualsSeparatorSwitch("--version-suffix", SwitchValueSeparator = " ")]
    public string? VersionSuffix { get; init; }

    [CommandEqualsSeparatorSwitch("--tl", SwitchValueSeparator = " ")]
    public string? TerminalLogger { get; init; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; init; }

    [BooleanCommandSwitch("--no-dependencies")]
    public bool? NoDependencies { get; init; }

    [BooleanCommandSwitch("--no-incremental")]
    public bool? NoIncremental { get; init; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; init; }

    [BooleanCommandSwitch("--nologo")]
    public bool? NoLogo { get; init; }

    [BooleanCommandSwitch("--no-self-contained")]
    public bool? NoSelfContained { get; init; }

    [BooleanCommandSwitch("--use-current-runtime")]
    public bool? UseCurrentRuntime { get; init; }
}