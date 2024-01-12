using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "ssl-policies", "create")]
public record GcloudComputeSslPoliciesCreateOptions(
[property: PositionalArgument] string SslPolicy
) : GcloudOptions
{
    [CommandSwitch("--custom-features")]
    public string[]? CustomFeatures { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}