using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("bugreport")]
public record GitBugreportOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [CommandEqualsSeparatorSwitch("--suffix")]
    public string? Suffix { get; set; }

    [BooleanCommandSwitch("--no-diagnose")]
    public bool? NoDiagnose { get; set; }

    [CommandEqualsSeparatorSwitch("--diagnose")]
    public string? Diagnose { get; set; }

}