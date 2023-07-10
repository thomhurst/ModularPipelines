using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("bugreport")]
public record GitBugreportOptions : GitOptions
{
    [CommandLongSwitch("output-directory")]
    public string? OutputDirectory { get; set; }

    [CommandLongSwitch("suffix")]
    public string? Suffix { get; set; }

    [BooleanCommandSwitch("no-diagnose")]
    public bool? NoDiagnose { get; set; }

    [CommandLongSwitch("diagnose")]
    public string? Diagnose { get; set; }

}