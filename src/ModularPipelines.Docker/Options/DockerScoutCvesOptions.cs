using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "cves")]
[ExcludeFromCodeCoverage]
public record DockerScoutCvesOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? ImageOrDirectoryOrArchive { get; set; }

    [CliFlag("--details")]
    public virtual bool? Details { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliOption("--exit-code")]
    public virtual string? ExitCode { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--ignore-base")]
    public virtual string? IgnoreBase { get; set; }

    [CliOption("--locations")]
    public virtual string? Locations { get; set; }

    [CliOption("--multi-stage")]
    public virtual string? MultiStage { get; set; }

    [CliOption("--only-cve-id")]
    public virtual string? OnlyCveId { get; set; }

    [CliOption("--only-fixed")]
    public virtual string? OnlyFixed { get; set; }

    [CliOption("--only-metric")]
    public virtual string? OnlyMetric { get; set; }

    [CliOption("--only-package")]
    public virtual string? OnlyPackage { get; set; }

    [CliOption("--only-package-type")]
    public virtual string? OnlyPackageType { get; set; }

    [CliOption("--only-severity")]
    public virtual string? OnlySeverity { get; set; }

    [CliOption("--only-stage")]
    public virtual string? OnlyStage { get; set; }

    [CliOption("--only-unfixed")]
    public virtual string? OnlyUnfixed { get; set; }

    [CliOption("--only-vex-affected")]
    public virtual string? OnlyVexAffected { get; set; }

    [CliOption("--only-vuln-packages")]
    public virtual string? OnlyVulnPackages { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--ref")]
    public virtual string? Ref { get; set; }

    [CliOption("--stream")]
    public virtual string? Stream { get; set; }

    [CliOption("--vex")]
    public virtual string? Vex { get; set; }

    [CliOption("--vex-author")]
    public virtual string? VexAuthor { get; set; }

    [CliOption("--vex-location")]
    public virtual string? VexLocation { get; set; }
}
