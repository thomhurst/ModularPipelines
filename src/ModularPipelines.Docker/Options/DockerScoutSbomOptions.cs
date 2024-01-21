using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "sbom")]
[ExcludeFromCodeCoverage]
public record DockerScoutSbomOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--only-package-type")]
    public string? OnlyPackageType { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }
}
