using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "update")]
public record GcloudContainerBinauthzAttestorsUpdateOptions(
[property: PositionalArgument] string Attestor
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}