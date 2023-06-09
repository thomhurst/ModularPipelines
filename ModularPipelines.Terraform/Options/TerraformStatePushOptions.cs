﻿using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state push")]
public record TerraformStatePushOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Path) : TerraformOptions
{
    [BooleanCommandSwitch("-force")] public bool? Force { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public bool? IgnoreRemoteVersion { get; set; }
}