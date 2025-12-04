using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tsi", "reference-data-set", "create")]
public record AzTsiReferenceDataSetCreateOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--key-properties")] string KeyProperties,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--comparison-behavior")]
    public string? ComparisonBehavior { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}