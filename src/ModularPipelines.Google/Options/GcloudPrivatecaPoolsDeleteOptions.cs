using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "delete")]
public record GcloudPrivatecaPoolsDeleteOptions(
[property: CliArgument] string CaPool,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--ignore-dependent-resources")]
    public bool? IgnoreDependentResources { get; set; }
}