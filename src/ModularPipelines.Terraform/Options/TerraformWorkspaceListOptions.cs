using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace list")]
public record TerraformWorkspaceListOptions : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Directory { get; set; }
}