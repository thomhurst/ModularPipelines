using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("test")]
[ExcludeFromCodeCoverage]
public record DotNetTestOptions : DotNetOptions
{
    [BooleanCommandSwitch("--blame")]
    public bool? Blame { get; init; }

    [BooleanCommandSwitch("--blame-crash")]
    public bool? BlameCrash { get; init; }

    [CommandEqualsSeparatorSwitch("--blame-crash-dump-type")]
    public string? BlameCrashDumpType { get; init; }

    [BooleanCommandSwitch("--blame-crash-collect-always")]
    public bool? BlameCrashCollectAlways { get; init; }

    [BooleanCommandSwitch("--blame-hang")]
    public bool? BlameHang { get; init; }

    [CommandEqualsSeparatorSwitch("--blame-hang-dump-type")]
    public string? BlameHangDumpType { get; init; }

    [CommandEqualsSeparatorSwitch("--blame-hang-timeout")]
    public string? BlameHangTimeout { get; init; }

    [CommandSwitch("-c")]
    public Configuration? Configuration { get; init; } = Options.Configuration.Release;

    [CommandSwitch("-f")]
    public string? Framework { get; init; }

    [CommandEqualsSeparatorSwitch("--filter", SwitchValueSeparator = " ")]
    public string? Filter { get; init; }

    [CommandEqualsSeparatorSwitch("--environment", SwitchValueSeparator = " ")]
    public string? Environment { get; init; }

    [CommandSwitch("-d")]
    public string? DiagnosticLogFile { get; init; }

    [CommandSwitch("-l")]
    public ICollection<string>? Logger { get; set; }

    [CommandEqualsSeparatorSwitch("--results-directory", SwitchValueSeparator = " ")]
    public string? ResultsDirectory { get; init; }

    [CommandSwitch("-s")]
    public string? SettingsFile { get; init; }

    [CommandEqualsSeparatorSwitch("--collect", SwitchValueSeparator = " ")]
    public string? Collect { get; init; }

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