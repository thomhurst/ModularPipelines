using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "cves")]
[ExcludeFromCodeCoverage]
public record DockerScoutCvesOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ImageOrDirectoryOrArchive { get; set; }

    [BooleanCommandSwitch("--details")]
    public virtual bool? Details { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [CommandSwitch("--exit-code")]
    public virtual string? ExitCode { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--ignore-base")]
    public virtual string? IgnoreBase { get; set; }

    [CommandSwitch("--locations")]
    public virtual string? Locations { get; set; }

    [CommandSwitch("--multi-stage")]
    public virtual string? MultiStage { get; set; }

    [CommandSwitch("--only-cve-id")]
    public virtual string? OnlyCveId { get; set; }

    [CommandSwitch("--only-fixed")]
    public virtual string? OnlyFixed { get; set; }

    [CommandSwitch("--only-metric")]
    public virtual string? OnlyMetric { get; set; }

    [CommandSwitch("--only-package")]
    public virtual string? OnlyPackage { get; set; }

    [CommandSwitch("--only-package-type")]
    public virtual string? OnlyPackageType { get; set; }

    [CommandSwitch("--only-severity")]
    public virtual string? OnlySeverity { get; set; }

    [CommandSwitch("--only-stage")]
    public virtual string? OnlyStage { get; set; }

    [CommandSwitch("--only-unfixed")]
    public virtual string? OnlyUnfixed { get; set; }

    [CommandSwitch("--only-vex-affected")]
    public virtual string? OnlyVexAffected { get; set; }

    [CommandSwitch("--only-vuln-packages")]
    public virtual string? OnlyVulnPackages { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--output")]
    public virtual string? Output { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public virtual string? Ref { get; set; }

    [CommandSwitch("--stream")]
    public virtual string? Stream { get; set; }

    [CommandSwitch("--vex")]
    public virtual string? Vex { get; set; }

    [CommandSwitch("--vex-author")]
    public virtual string? VexAuthor { get; set; }

    [CommandSwitch("--vex-location")]
    public virtual string? VexLocation { get; set; }
}
