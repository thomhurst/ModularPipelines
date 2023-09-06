using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace list")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceListOptions : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Directory { get; set; }
}
