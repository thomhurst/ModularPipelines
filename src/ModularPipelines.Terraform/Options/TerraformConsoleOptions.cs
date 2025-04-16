using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("console")]
[ExcludeFromCodeCoverage]
public record TerraformConsoleOptions : TerraformOptions
{
    [BooleanCommandSwitch("-state")]
    public virtual bool? State { get; set; }
}