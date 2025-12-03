using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("image", "builder", "trigger", "list")]
public record AzImageBuilderTriggerListOptions(
[property: CliOption("--image-template-name")] string ImageTemplateName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;