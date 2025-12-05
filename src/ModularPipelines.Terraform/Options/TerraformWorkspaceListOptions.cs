using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("workspace list")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceListOptions : TerraformOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Directory { get; set; }
}