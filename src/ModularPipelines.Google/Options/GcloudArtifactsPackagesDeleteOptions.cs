using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "packages", "delete")]
public record GcloudArtifactsPackagesDeleteOptions(
[property: PositionalArgument] string Package,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Repository
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}