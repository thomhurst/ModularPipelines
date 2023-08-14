using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace delete")]
public record TerraformWorkspaceDeleteOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Name) : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Directory { get; set; }

    [BooleanCommandSwitch("-force")] public bool? Force { get; set; }

    [BooleanCommandSwitch("-lock")] public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")] public string? LockTimeout { get; set; }
}