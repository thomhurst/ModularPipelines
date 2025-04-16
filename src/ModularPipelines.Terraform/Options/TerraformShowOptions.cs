using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("show")]
[ExcludeFromCodeCoverage]
public record TerraformShowOptions : TerraformOptions
{
    [BooleanCommandSwitch("-json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("-refresh")]
    public virtual bool? Refresh { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public virtual bool? NoColor { get; set; }
}