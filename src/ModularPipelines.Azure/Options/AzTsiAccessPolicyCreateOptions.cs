using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "access-policy", "create")]
public record AzTsiAccessPolicyCreateOptions(
[property: CommandSwitch("--access-policy-name")] string AccessPolicyName,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--principal-object-id")]
    public string? PrincipalObjectId { get; set; }

    [CommandSwitch("--roles")]
    public string? Roles { get; set; }
}