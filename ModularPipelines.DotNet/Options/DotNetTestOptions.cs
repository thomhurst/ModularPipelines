using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

public record DotNetTestOptions : DotNetOptions
{
    public IEnumerable<string>? RunSettingsArguments { get; init; }

    [BooleanCommandSwitch("blame")]
    public bool? Blame { get; init; }

    [BooleanCommandSwitch("blame-crash")]
    public bool? BlameCrash { get; init; }

    [CommandLongSwitch("blame-crash-dump-type")]
    public string? BlameCrashDumpType { get; init; }

    [BooleanCommandSwitch("blame-crash-collect-always")]
    public bool? BlameCrashCollectAlways{ get; init; }

    [BooleanCommandSwitch("blame-hang")]
    public bool? BlameHang { get; init; }

    [CommandLongSwitch("blame-hang-dump-type")]
    public string? BlameHangDumpType { get; init; }

    [CommandLongSwitch("blame-hang-timeout")]
    public string? BlameHangTimeout { get; init; }

    [CommandSwitch("c")]
    public Configuration? Configuration { get; init; }

    [CommandSwitch("f")]
    public string? Framework { get; init; }
    
    [CommandLongSwitch("filter", SwitchValueSeparator = " ")]
    public string? Filter { get; init; }

    [CommandLongSwitch("environment", SwitchValueSeparator = " ")]
    public string? Environment { get; init; }

    [CommandSwitch("d")]
    public string? DiagnosticLogFile { get; init; }
    
    [CommandSwitch("l")]
    public ICollection<string>? Logger { get; set; }
    
    [CommandLongSwitch("results-directory", SwitchValueSeparator = " ")]
    public string? ResultsDirectory { get; init; }

    [CommandSwitch("s")]
    public string? SettingsFile { get; init; }

    [CommandLongSwitch("collect", SwitchValueSeparator = " ")]
    public string? Collect { get; init; }

    [CommandSwitch("a")]
    public string? Architecture { get; init; }
    
    [CommandSwitch("o")]
    public string? Output { get; init; }
    
    [CommandSwitch("s")]
    public string? Source { get; init; }
    
    [CommandLongSwitch("os", SwitchValueSeparator = " ")]
    public string? OperatingSystem { get; init; }
    
    [CommandLongSwitch("version-suffix", SwitchValueSeparator = " ")]
    public string? VersionSuffix { get; init; }
    
    [CommandLongSwitch("tl", SwitchValueSeparator = " ")]
    public string? TerminalLogger { get; init; }
    
    [BooleanCommandSwitch("force")]
    public bool? Force { get; init; }
    
    [BooleanCommandSwitch("no-dependencies")]
    public bool? NoDependencies { get; init; }
    
    [BooleanCommandSwitch("no-incremental")]
    public bool? NoIncremental { get; init; }
    
    [BooleanCommandSwitch("no-restore")]
    public bool? NoRestore { get; init; }
    
    [BooleanCommandSwitch("nologo")]
    public bool? NoLogo { get; init; }
    
    [BooleanCommandSwitch("no-self-contained")]
    public bool? NoSelfContained { get; init; }
    
    [BooleanCommandSwitch("use-current-runtime")]
    public bool? UseCurrentRuntime { get; init; }
}