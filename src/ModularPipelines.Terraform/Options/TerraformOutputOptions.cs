using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("output")]
[ExcludeFromCodeCoverage]
public record TerraformOutputOptions : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [BooleanCommandSwitch("-json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("-raw")]
    public bool? Raw { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public bool? NoColor { get; set; }

    [CommandSwitch("-state")]
    public string? State { get; set; }
}