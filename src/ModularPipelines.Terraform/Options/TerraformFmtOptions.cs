using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("fmt")]
[ExcludeFromCodeCoverage]
public record TerraformFmtOptions : TerraformOptions
{
    [CliFlag("-list")]
    public virtual bool? List { get; set; }

    [CliFlag("-write")]
    public virtual bool? Write { get; set; }

    [CliFlag("-check")]
    public virtual bool? Check { get; set; }

    [CliFlag("-diff")]
    public virtual bool? Diff { get; set; }

    [CliFlag("-recursive")]
    public virtual bool? Recursive { get; set; }
}