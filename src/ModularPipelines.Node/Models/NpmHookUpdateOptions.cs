﻿using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hook", "update")]
public record NpmHookUpdateOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Id,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Url,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Secret
) : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }
}