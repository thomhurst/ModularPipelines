using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("validate")]
[ExcludeFromCodeCoverage]
public record TerraformValidateOptions : TerraformOptions
{
    [BooleanCommandSwitch("-json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public virtual bool? NoColor { get; set; }
}