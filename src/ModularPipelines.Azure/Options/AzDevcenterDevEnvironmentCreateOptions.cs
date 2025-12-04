using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "dev", "environment", "create")]
public record AzDevcenterDevEnvironmentCreateOptions(
[property: CliOption("--catalog-name")] string CatalogName,
[property: CliOption("--environment-definition-name")] string EnvironmentDefinitionName,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--environment-type")] string EnvironmentType,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}