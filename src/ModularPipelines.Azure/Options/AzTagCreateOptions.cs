using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tag", "create")]
public record AzTagCreateOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}