using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("restore")]
public record DotNetRestoreOptions : DotNetOptions
{
    [CommandSwitch("-c")]
    public Configuration? Configuration { get; init; }

    [CommandSwitch("-f")]
    public string? Framework { get; init; }

    [CommandSwitch("-a")]
    public string? Architecture { get; init; }

    [CommandSwitch("-o")]
    public string? Output { get; init; }

    [CommandSwitch("-s")]
    public string? Source { get; init; }

    [CommandEqualsSeparatorSwitch("--config-file", SwitchValueSeparator = " ")]
    public string? ConfigFile { get; init; }

    [CommandEqualsSeparatorSwitch("--packages", SwitchValueSeparator = " ")]
    public string? PackagesDirectory { get; init; }

    [BooleanCommandSwitch("--force-evaluate")]
    public bool? ForceEvaluate { get; init; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public bool? IgnoreFailedSources { get; init; }

    [BooleanCommandSwitch("--disable-parallel")]
    public bool? DisableParallel { get; init; }

    [BooleanCommandSwitch("--locked-mode")]
    public bool? LockedMode { get; init; }

    [BooleanCommandSwitch("--use-lock-file")]
    public bool? UseLockFile { get; init; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; init; }

    [BooleanCommandSwitch("--nologo")]
    public bool? NoLogo { get; init; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; init; }

    [BooleanCommandSwitch("--use-current-runtime")]
    public bool? UseCurrentRuntime { get; init; }
}
