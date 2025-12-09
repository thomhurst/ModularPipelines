using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "show-deleted")]
public record AzAppconfigShowDeletedOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}