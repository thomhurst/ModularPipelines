using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource", "link", "update")]
public record AzResourceLinkUpdateOptions(
[property: CliOption("--link")] string Link
) : AzOptions
{
    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }
}