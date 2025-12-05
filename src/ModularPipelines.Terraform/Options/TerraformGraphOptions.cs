using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("graph")]
[ExcludeFromCodeCoverage]
public record TerraformGraphOptions : TerraformOptions
{
    [CliFlag("-type")]
    public virtual bool? Type { get; set; }

    [CliOption("-plan")]
    public virtual string? Plan { get; set; }

    [CliFlag("-draw-cycles")]
    public virtual bool? DrawCycles { get; set; }

    [CliOption("-module-depth")]
    public virtual int? ModuleDepth { get; set; }
}