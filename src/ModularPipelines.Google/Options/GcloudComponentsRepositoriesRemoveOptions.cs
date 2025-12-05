using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("components", "repositories", "remove")]
public record GcloudComponentsRepositoriesRemoveOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }
}