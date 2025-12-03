using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource", "link", "delete")]
public record AzResourceLinkDeleteOptions(
[property: CliOption("--link")] string Link
) : AzOptions;