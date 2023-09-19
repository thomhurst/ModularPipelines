using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state show")]
[ExcludeFromCodeCoverage]
public record TerraformStateShowOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Address) : TerraformOptions
{
    [CommandSwitch("-state")] public string? State { get; set; }
}
