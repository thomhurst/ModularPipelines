﻿using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin")]
public record DockerPluginOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}