using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace select")]
public record TerraformWorkspaceSelectOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Name) : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Directory { get; set; }

    [BooleanCommandSwitch("-or-create")] public bool? OrCreate { get; set; }
}