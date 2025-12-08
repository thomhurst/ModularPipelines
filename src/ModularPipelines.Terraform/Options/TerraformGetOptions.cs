using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("get")]
[ExcludeFromCodeCoverage]
public record TerraformGetOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Path) : TerraformOptions
{
    [CliFlag("-update")]
    public virtual bool? Update { get; set; }

    [CliFlag("-no-color")]
    public virtual bool? NoColor { get; set; }
}