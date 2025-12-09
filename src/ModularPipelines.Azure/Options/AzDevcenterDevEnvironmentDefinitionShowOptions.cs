using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "dev", "environment-definition", "show")]
public record AzDevcenterDevEnvironmentDefinitionShowOptions(
[property: CliOption("--catalog-name")] string CatalogName,
[property: CliOption("--definition-name")] string DefinitionName,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }
}