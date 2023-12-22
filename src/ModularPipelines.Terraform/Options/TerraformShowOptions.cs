using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("show")]
[ExcludeFromCodeCoverage]
public record TerraformShowOptions : TerraformOptions
{
    [BooleanCommandSwitch("-json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("-refresh")]
    public bool? Refresh { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public bool? NoColor { get; set; }
}