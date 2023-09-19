﻿using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout sbom")]
[ExcludeFromCodeCoverage]
public record DockerScoutSbomOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Image { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--only-package-type")]
    public string? OnlyPackageType { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}
