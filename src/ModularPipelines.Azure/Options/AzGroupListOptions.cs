using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "list")]
public record AzGroupListOptions : AzOptions
{
    [CliOption("--tag")]
    public string? Tag { get; set; }
}