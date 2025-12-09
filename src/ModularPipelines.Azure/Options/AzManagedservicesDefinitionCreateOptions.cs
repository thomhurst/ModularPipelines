using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managedservices", "definition", "create")]
public record AzManagedservicesDefinitionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--role-definition-id")] string RoleDefinitionId,
[property: CliOption("--tenant-id")] string TenantId
) : AzOptions
{
    [CliOption("--definition-id")]
    public string? DefinitionId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--plan-version")]
    public string? PlanVersion { get; set; }
}