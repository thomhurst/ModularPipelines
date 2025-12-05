using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "revision", "label", "add")]
public record AzContainerappRevisionLabelAddOptions(
[property: CliOption("--label")] string Label,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--revision")] string Revision
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-prompt")]
    public bool? NoPrompt { get; set; }
}