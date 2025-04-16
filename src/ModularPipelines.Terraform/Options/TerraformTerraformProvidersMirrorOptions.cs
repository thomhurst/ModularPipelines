using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("terraform providers mirror")]
[ExcludeFromCodeCoverage]
public record TerraformTerraformProvidersMirrorOptions : TerraformOptions
{
    [CommandSwitch("-platform")]
    public virtual string? Platform { get; set; }
}