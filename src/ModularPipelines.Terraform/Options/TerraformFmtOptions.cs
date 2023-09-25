using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("fmt")]
[ExcludeFromCodeCoverage]
public record TerraformFmtOptions : TerraformOptions
{
    [BooleanCommandSwitch("-list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("-write")]
    public bool? Write { get; set; }

    [BooleanCommandSwitch("-check")]
    public bool? Check { get; set; }

    [BooleanCommandSwitch("-diff")]
    public bool? Diff { get; set; }

    [BooleanCommandSwitch("-recursive")]
    public bool? Recursive { get; set; }
}
