using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("workspace list")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceListOptions : TerraformOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Directory { get; set; }
}