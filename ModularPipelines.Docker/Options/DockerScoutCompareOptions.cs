using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout compare")]
public record DockerScoutCompareOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Image) : DockerOptions
{

    [CommandSwitch("--exit-code")]
    public string? ExitCode { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--ignore-base")]
    public string? IgnoreBase { get; set; }

    [CommandSwitch("--ignore-unchanged")]
    public string? IgnoreUnchanged { get; set; }

    [CommandSwitch("--only-fixed")]
    public string? OnlyFixed { get; set; }

    [CommandSwitch("--only-package-type")]
    public string? OnlyPackageType { get; set; }

    [CommandSwitch("--only-severity")]
    public string? OnlySeverity { get; set; }

    [CommandSwitch("--only-unfixed")]
    public string? OnlyUnfixed { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }

    [CommandSwitch("--to")]
    public string? To { get; set; }

    [CommandSwitch("--to-latest")]
    public string? ToLatest { get; set; }

    [CommandSwitch("--to-ref")]
    public string? ToRef { get; set; }

    [CommandSwitch("--to-stream")]
    public string? ToStream { get; set; }

    [CommandSwitch("--to-type")]
    public string? ToType { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

}