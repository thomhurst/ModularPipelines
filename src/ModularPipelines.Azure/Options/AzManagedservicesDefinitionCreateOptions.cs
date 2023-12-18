using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "definition", "create")]
public record AzManagedservicesDefinitionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--role-definition-id")] string RoleDefinitionId,
[property: CommandSwitch("--tenant-id")] string TenantId
) : AzOptions
{
    [CommandSwitch("--definition-id")]
    public string? DefinitionId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--plan-name")]
    public string? PlanName { get; set; }

    [CommandSwitch("--plan-product")]
    public string? PlanProduct { get; set; }

    [CommandSwitch("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CommandSwitch("--plan-version")]
    public string? PlanVersion { get; set; }
}