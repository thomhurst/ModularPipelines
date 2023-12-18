using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "environment-definition", "list")]
public record AzDevcenterDevEnvironmentDefinitionListOptions(
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--catalog-name")]
    public string? CatalogName { get; set; }

    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }
}