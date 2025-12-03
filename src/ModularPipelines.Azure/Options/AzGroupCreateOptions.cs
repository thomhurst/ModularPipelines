using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "create")]
public record AzGroupCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--managed-by")]
    public string? ManagedBy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}