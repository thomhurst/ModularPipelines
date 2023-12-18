using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "environment", "create")]
public record AzDevcenterDevEnvironmentCreateOptions(
[property: CommandSwitch("--catalog-name")] string CatalogName,
[property: CommandSwitch("--environment-definition-name")] string EnvironmentDefinitionName,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--environment-type")] string EnvironmentType,
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }
}

