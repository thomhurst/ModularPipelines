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
    public bool? Details { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--exit-code")]
    public string? ExitCode { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--ignore-base")]
    public string? IgnoreBase { get; set; }

    [CommandSwitch("--locations")]
    public string? Locations { get; set; }

    [CommandSwitch("--multi-stage")]
    public string? MultiStage { get; set; }

    [CommandSwitch("--only-cve-id")]
    public string? OnlyCveId { get; set; }

    [CommandSwitch("--only-fixed")]
    public string? OnlyFixed { get; set; }

    [CommandSwitch("--only-metric")]
    public string? OnlyMetric { get; set; }

    [CommandSwitch("--only-package")]
    public string? OnlyPackage { get; set; }

    [CommandSwitch("--only-package-type")]
    public string? OnlyPackageType { get; set; }

    [CommandSwitch("--only-severity")]
    public string? OnlySeverity { get; set; }

    [CommandSwitch("--only-stage")]
    public string? OnlyStage { get; set; }

    [CommandSwitch("--only-unfixed")]
    public string? OnlyUnfixed { get; set; }

    [CommandSwitch("--only-vex-affected")]
    public string? OnlyVexAffected { get; set; }

    [CommandSwitch("--only-vuln-packages")]
    public string? OnlyVulnPackages { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }

    [CommandSwitch("--stream")]
    public string? Stream { get; set; }

    [CommandSwitch("--vex")]
    public string? Vex { get; set; }

    [CommandSwitch("--vex-author")]
    public string? VexAuthor { get; set; }

    [CommandSwitch("--vex-location")]
    public string? VexLocation { get; set; }
}
