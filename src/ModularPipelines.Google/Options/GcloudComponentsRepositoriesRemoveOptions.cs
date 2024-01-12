using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("components", "repositories", "remove")]
public record GcloudComponentsRepositoriesRemoveOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}