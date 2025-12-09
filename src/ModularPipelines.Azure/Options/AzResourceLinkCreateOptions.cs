using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource", "link", "create")]
public record AzResourceLinkCreateOptions(
[property: CliOption("--link")] string Link,
[property: CliOption("--target")] string Target
) : AzOptions
{
    [CliOption("--notes")]
    public string? Notes { get; set; }
}