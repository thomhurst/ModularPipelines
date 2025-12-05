using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "ssl-policies", "update")]
public record GcloudComputeSslPoliciesUpdateOptions(
[property: CliArgument] string SslPolicy
) : GcloudOptions
{
    [CliOption("--custom-features")]
    public string[]? CustomFeatures { get; set; }

    [CliOption("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}