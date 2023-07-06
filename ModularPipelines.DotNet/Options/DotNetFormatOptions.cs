using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("format")]
public record DotNetFormatOptions : DotNetOptions
{
    [CommandLongSwitch("diagnostics", SwitchValueSeparator = " ")]
    public string Diagnostics { get; init; }

    [BooleanCommandSwitch("severity")]
    public string Severity { get; init; }

    [BooleanCommandSwitch("no-restore")]
    public bool NoRestore { get; init; }

    [BooleanCommandSwitch("verify-no-changes")]
    public bool VerifyNoChanges { get; init; } = true;

    [CommandLongSwitch("include", SwitchValueSeparator = " ")]
    public string Include { get; init; }

    [CommandLongSwitch("exclude", SwitchValueSeparator = " ")]
    public string Exclude { get; init; }

    [BooleanCommandSwitch("include-generated")]
    public bool IncludeGenerated { get; init; }

    [CommandLongSwitch("binary-log", SwitchValueSeparator = " ")]
    public string BinaryLogPath { get; init; }

    [CommandLongSwitch("report", SwitchValueSeparator = " ")]
    public string ReportPath { get; init; }
}
