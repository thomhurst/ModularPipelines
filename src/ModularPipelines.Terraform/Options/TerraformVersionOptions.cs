using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("version")]
[ExcludeFromCodeCoverage]
public record TerraformVersionOptions : TerraformOptions
{
    [BooleanCommandSwitch("-json")]
    public bool? Json { get; set; }
}
