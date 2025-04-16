using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state list")]
[ExcludeFromCodeCoverage]
public record TerraformStateListOptions : TerraformOptions
{
    [CommandSwitch("-state")]
    public virtual string? State { get; set; }

    [CommandSwitch("-id")]
    public virtual string? Id { get; set; }
}