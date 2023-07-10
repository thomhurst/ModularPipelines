using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout compare")]
public record DockerScoutCompareOptions : DockerOptions
{
    [CommandLongSwitch("exit-code")]
    public string? ExitCode { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("ignore-base")]
    public string? IgnoreBase { get; set; }

    [CommandLongSwitch("ignore-unchanged")]
    public string? IgnoreUnchanged { get; set; }

    [CommandLongSwitch("only-fixed")]
    public string? OnlyFixed { get; set; }

    [CommandLongSwitch("only-package-type")]
    public string? OnlyPackageType { get; set; }

    [CommandLongSwitch("only-severity")]
    public string? OnlySeverity { get; set; }

    [CommandLongSwitch("only-unfixed")]
    public string? OnlyUnfixed { get; set; }

    [CommandLongSwitch("output")]
    public string? Output { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

    [CommandLongSwitch("ref")]
    public string? Ref { get; set; }

    [CommandLongSwitch("to")]
    public string? To { get; set; }

    [CommandLongSwitch("to-latest")]
    public string? ToLatest { get; set; }

    [CommandLongSwitch("to-ref")]
    public string? ToRef { get; set; }

    [CommandLongSwitch("to-stream")]
    public string? ToStream { get; set; }

    [CommandLongSwitch("to-type")]
    public string? ToType { get; set; }

    [CommandLongSwitch("type")]
    public string? Type { get; set; }

}