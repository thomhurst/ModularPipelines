using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace list")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceListOptions : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Directory { get; set; }
}
