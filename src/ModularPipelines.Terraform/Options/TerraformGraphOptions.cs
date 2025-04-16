using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("graph")]
[ExcludeFromCodeCoverage]
public record TerraformGraphOptions : TerraformOptions
{
    [BooleanCommandSwitch("-type")]
    public virtual bool? Type { get; set; }

    [CommandSwitch("-plan")]
    public virtual string? Plan { get; set; }

    [BooleanCommandSwitch("-draw-cycles")]
    public virtual bool? DrawCycles { get; set; }

    [CommandSwitch("-module-depth")]
    public virtual int? ModuleDepth { get; set; }
}