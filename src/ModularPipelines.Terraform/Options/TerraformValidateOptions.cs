using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("validate")]
[ExcludeFromCodeCoverage]
public record TerraformValidateOptions : TerraformOptions
{
    [BooleanCommandSwitch("-json")] public bool? Json { get; set; }

    [BooleanCommandSwitch("-no-color")] public bool? NoColor { get; set; }
}
