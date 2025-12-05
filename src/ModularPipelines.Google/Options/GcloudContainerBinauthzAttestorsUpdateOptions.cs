using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "update")]
public record GcloudContainerBinauthzAttestorsUpdateOptions(
[property: CliArgument] string Attestor
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}