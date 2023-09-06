using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("console")]
[ExcludeFromCodeCoverage]
public record TerraformConsoleOptions : TerraformOptions
{
    [BooleanCommandSwitch("-state")] public bool? State { get; set; }
}
