using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "environment-definition", "show")]
public record AzDevcenterAdminEnvironmentDefinitionShowOptions : AzOptions
{
    [CliOption("--catalog-name")]
    public string? CatalogName { get; set; }

    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--environment-definition-name")]
    public string? EnvironmentDefinitionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}