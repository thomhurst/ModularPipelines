using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("workspace select")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceSelectOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Name) : TerraformOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Directory { get; set; }

    [CliFlag("-or-create")]
    public virtual bool? OrCreate { get; set; }
}