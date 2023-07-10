using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout cves")]
public record DockerScoutCvesOptions : DockerOptions
{
    [CommandLongSwitch("details")]
    public string? Details { get; set; }

    [CommandLongSwitch("exit-code")]
    public string? ExitCode { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("ignore-base")]
    public string? IgnoreBase { get; set; }

    [CommandLongSwitch("locations")]
    public string? Locations { get; set; }

    [CommandLongSwitch("only-cve-id")]
    public string? OnlyCveId { get; set; }

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

    [CommandLongSwitch("type")]
    public string? Type { get; set; }

}