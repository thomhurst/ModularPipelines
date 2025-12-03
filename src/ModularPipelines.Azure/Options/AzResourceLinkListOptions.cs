using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource", "link", "list")]
public record AzResourceLinkListOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}