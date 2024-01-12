using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "pools", "delete")]
public record GcloudPrivatecaPoolsDeleteOptions(
[property: PositionalArgument] string CaPool,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--ignore-dependent-resources")]
    public bool? IgnoreDependentResources { get; set; }
}