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
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("-raw")]
    public virtual bool? Raw { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public virtual bool? NoColor { get; set; }

    [CommandSwitch("-state")]
    public virtual string? State { get; set; }
}