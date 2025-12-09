using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "location", "list-skus")]
public record AzBatchLocationListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--maxresults")]
    public string? Maxresults { get; set; }
}