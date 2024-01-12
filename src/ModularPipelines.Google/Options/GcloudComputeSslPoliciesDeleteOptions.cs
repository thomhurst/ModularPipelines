using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "ssl-policies", "delete")]
public record GcloudComputeSslPoliciesDeleteOptions(
[property: PositionalArgument] string SslPolicy
) : GcloudOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}