using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "dev", "environment-definition", "list")]
public record AzDevcenterDevEnvironmentDefinitionListOptions(
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--catalog-name")]
    public string? CatalogName { get; set; }

    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }
}