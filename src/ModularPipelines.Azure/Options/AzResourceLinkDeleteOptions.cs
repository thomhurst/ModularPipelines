using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource", "link", "delete")]
public record AzResourceLinkDeleteOptions(
[property: CommandSwitch("--link")] string Link
) : AzOptions;