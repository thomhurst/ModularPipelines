using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource", "link", "show")]
public record AzResourceLinkShowOptions(
[property: CommandSwitch("--link")] string Link
) : AzOptions;