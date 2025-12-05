using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource", "link", "show")]
public record AzResourceLinkShowOptions(
[property: CliOption("--link")] string Link
) : AzOptions;