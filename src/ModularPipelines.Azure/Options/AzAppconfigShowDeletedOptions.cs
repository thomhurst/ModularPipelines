using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "show-deleted")]
public record AzAppconfigShowDeletedOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}