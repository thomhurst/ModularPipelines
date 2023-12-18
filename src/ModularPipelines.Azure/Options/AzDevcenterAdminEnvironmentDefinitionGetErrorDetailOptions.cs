using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "environment-definition", "get-error-detail")]
public record AzDevcenterAdminEnvironmentDefinitionGetErrorDetailOptions(
[property: CommandSwitch("--catalog-name")] string CatalogName,
[property: CommandSwitch("--dev-center")] string DevCenter,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--environment-definition-name")]
    public string? EnvironmentDefinitionName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}