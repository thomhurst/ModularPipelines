using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "use-dev-spaces", "(dev-spaces", "extension)")]
public record AzAksUseDevSpacesDevSpacesExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--space")]
    public string? Space { get; set; }

    [CliFlag("--update")]
    public bool? Update { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}