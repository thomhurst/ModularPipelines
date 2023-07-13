﻿using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("save")]
public record DockerSaveOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Image) : DockerOptions
{
    [CommandSwitch("--output")]
    public string? Output { get; set; }
}