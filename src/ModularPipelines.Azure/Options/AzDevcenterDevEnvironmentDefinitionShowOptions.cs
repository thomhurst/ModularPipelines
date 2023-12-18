using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "environment-definition", "show")]
public record AzDevcenterDevEnvironmentDefinitionShowOptions(
[property: CommandSwitch("--catalog-name")] string CatalogName,
[property: CommandSwitch("--definition-name")] string DefinitionName,
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }
}