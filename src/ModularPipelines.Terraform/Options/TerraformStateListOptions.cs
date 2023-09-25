using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state list")]
[ExcludeFromCodeCoverage]
public record TerraformStateListOptions : TerraformOptions
{
    [CommandSwitch("-state")]
    public string? State { get; set; }

    [CommandSwitch("-id")]
    public string? Id { get; set; }
}
