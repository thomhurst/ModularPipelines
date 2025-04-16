using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("fmt")]
[ExcludeFromCodeCoverage]
public record TerraformFmtOptions : TerraformOptions
{
    [BooleanCommandSwitch("-list")]
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("-write")]
    public virtual bool? Write { get; set; }

    [BooleanCommandSwitch("-check")]
    public virtual bool? Check { get; set; }

    [BooleanCommandSwitch("-diff")]
    public virtual bool? Diff { get; set; }

    [BooleanCommandSwitch("-recursive")]
    public virtual bool? Recursive { get; set; }
}