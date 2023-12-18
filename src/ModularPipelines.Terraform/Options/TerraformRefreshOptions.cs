using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("refresh")]
[ExcludeFromCodeCoverage]
public record TerraformRefreshOptions : TerraformOptions
{
    [BooleanCommandSwitch("-auto-approve")]
    public bool? AutoApprove { get; set; }

    [BooleanCommandSwitch("-refresh-only")]
    public bool? RefreshOnly { get; set; }
}