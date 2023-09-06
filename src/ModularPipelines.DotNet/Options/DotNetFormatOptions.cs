using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("format")]
[ExcludeFromCodeCoverage]
public record DotNetFormatOptions : DotNetOptions
{
    [CommandEqualsSeparatorSwitch("--diagnostics", SwitchValueSeparator = " ")]
    public string? Diagnostics { get; init; }

    [BooleanCommandSwitch("--severity")]
    public string? Severity { get; init; }

    [BooleanCommandSwitch("--no-restore")]
    public bool NoRestore { get; init; }

    [BooleanCommandSwitch("--verify-no-changes")]
    public bool VerifyNoChanges { get; init; } = true;

    [CommandEqualsSeparatorSwitch("--include", SwitchValueSeparator = " ")]
    public string? Include { get; init; }

    [CommandEqualsSeparatorSwitch("--exclude", SwitchValueSeparator = " ")]
    public string? Exclude { get; init; }

    [BooleanCommandSwitch("--include-generated")]
    public bool IncludeGenerated { get; init; }

    [CommandEqualsSeparatorSwitch("--binary-log", SwitchValueSeparator = " ")]
    public string? BinaryLogPath { get; init; }

    [CommandEqualsSeparatorSwitch("--report", SwitchValueSeparator = " ")]
    public string? ReportPath { get; init; }
}
