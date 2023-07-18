using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout cves")]
public record DockerScoutCvesOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Image { get; set; }
    [BooleanCommandSwitch("--details")]
    public bool? Details { get; set; }

    [CommandSwitch("--exit-code")]
    public string? ExitCode { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--ignore-base")]
    public string? IgnoreBase { get; set; }

    [CommandSwitch("--locations")]
    public string? Locations { get; set; }

    [CommandSwitch("--only-cve-id")]
    public string? OnlyCveId { get; set; }

    [CommandSwitch("--only-fixed")]
    public string? OnlyFixed { get; set; }

    [CommandSwitch("--only-package-type")]
    public string? OnlyPackageType { get; set; }

    [CommandSwitch("--only-severity")]
    public string? OnlySeverity { get; set; }

    [CommandSwitch("--only-unfixed")]
    public string? OnlyUnfixed { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}