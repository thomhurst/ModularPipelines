using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "compare")]
[ExcludeFromCodeCoverage]
public record DockerScoutCompareOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? ImageOrDirectoryOrArchive { get; set; }

    [CliOption("--exit-code")]
    public virtual string? ExitCode { get; set; }

    [CliOption("--exit-on")]
    public virtual string? ExitOn { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--hide-policies")]
    public virtual string? HidePolicies { get; set; }

    [CliOption("--ignore-base")]
    public virtual string? IgnoreBase { get; set; }

    [CliOption("--ignore-unchanged")]
    public virtual string? IgnoreUnchanged { get; set; }

    [CliOption("--multi-stage")]
    public virtual string? MultiStage { get; set; }

    [CliOption("--only-fixed")]
    public virtual string? OnlyFixed { get; set; }

    [CliOption("--only-package-type")]
    public virtual string? OnlyPackageType { get; set; }

    [CliOption("--only-severity")]
    public virtual string? OnlySeverity { get; set; }

    [CliOption("--only-stage")]
    public virtual string? OnlyStage { get; set; }

    [CliOption("--only-unfixed")]
    public virtual string? OnlyUnfixed { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--ref")]
    public virtual string? Ref { get; set; }

    [CliOption("--to")]
    public virtual string? To { get; set; }

    [CliOption("--to-env")]
    public virtual string? ToEnv { get; set; }

    [CliOption("--to-latest")]
    public virtual string? ToLatest { get; set; }

    [CliOption("--to-ref")]
    public virtual string? ToRef { get; set; }

    [CliOption("--to-stream")]
    public virtual string? ToStream { get; set; }
}
