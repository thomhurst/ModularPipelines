using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tag", "list")]
public record AzTagListOptions : AzOptions
{
    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }
}