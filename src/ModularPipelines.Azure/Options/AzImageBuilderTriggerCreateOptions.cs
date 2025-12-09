using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("image", "builder", "trigger", "create")]
public record AzImageBuilderTriggerCreateOptions(
[property: CliOption("--image-template-name")] string ImageTemplateName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}