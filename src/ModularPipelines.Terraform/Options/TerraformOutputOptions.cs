using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("output")]
[ExcludeFromCodeCoverage]
public record TerraformOutputOptions : TerraformOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Name { get; set; }

    [CliFlag("-json")]
    public virtual bool? Json { get; set; }

    [CliFlag("-raw")]
    public virtual bool? Raw { get; set; }

    [CliFlag("-no-color")]
    public virtual bool? NoColor { get; set; }

    [CliOption("-state")]
    public virtual string? State { get; set; }
}