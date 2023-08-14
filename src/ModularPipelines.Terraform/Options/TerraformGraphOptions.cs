using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("graph")]
public record TerraformGraphOptions : TerraformOptions
{
    [BooleanCommandSwitch("-type")] public bool? Type { get; set; }

    [CommandSwitch("-plan")] public string? Plan { get; set; }

    [BooleanCommandSwitch("-draw-cycles")] public bool? DrawCycles { get; set; }

    [CommandSwitch("-module-depth")] public int? ModuleDepth { get; set; }
}