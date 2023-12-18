using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("State Command")]
[ExcludeFromCodeCoverage]
public record TerraformStateCommandOptions : TerraformOptions
{
    [BooleanCommandSwitch("-backup")]
    public bool? Backup { get; set; }
}