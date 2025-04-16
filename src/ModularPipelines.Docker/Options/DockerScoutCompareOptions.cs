using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "compare")]
[ExcludeFromCodeCoverage]
public record DockerScoutCompareOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ImageOrDirectoryOrArchive { get; set; }

    [CommandSwitch("--exit-code")]
    public virtual string? ExitCode { get; set; }

    [CommandSwitch("--exit-on")]
    public virtual string? ExitOn { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--hide-policies")]
    public virtual string? HidePolicies { get; set; }

    [CommandSwitch("--ignore-base")]
    public virtual string? IgnoreBase { get; set; }

    [CommandSwitch("--ignore-unchanged")]
    public virtual string? IgnoreUnchanged { get; set; }

    [CommandSwitch("--multi-stage")]
    public virtual string? MultiStage { get; set; }

    [CommandSwitch("--only-fixed")]
    public virtual string? OnlyFixed { get; set; }

    [CommandSwitch("--only-package-type")]
    public virtual string? OnlyPackageType { get; set; }

    [CommandSwitch("--only-severity")]
    public virtual string? OnlySeverity { get; set; }

    [CommandSwitch("--only-stage")]
    public virtual string? OnlyStage { get; set; }

    [CommandSwitch("--only-unfixed")]
    public virtual string? OnlyUnfixed { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--output")]
    public virtual string? Output { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public virtual string? Ref { get; set; }

    [CommandSwitch("--to")]
    public virtual string? To { get; set; }

    [CommandSwitch("--to-env")]
    public virtual string? ToEnv { get; set; }

    [CommandSwitch("--to-latest")]
    public virtual string? ToLatest { get; set; }

    [CommandSwitch("--to-ref")]
    public virtual string? ToRef { get; set; }

    [CommandSwitch("--to-stream")]
    public virtual string? ToStream { get; set; }
}
