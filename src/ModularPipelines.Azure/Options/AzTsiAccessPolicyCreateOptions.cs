using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tsi", "access-policy", "create")]
public record AzTsiAccessPolicyCreateOptions(
[property: CliOption("--access-policy-name")] string AccessPolicyName,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--principal-object-id")]
    public string? PrincipalObjectId { get; set; }

    [CliOption("--roles")]
    public string? Roles { get; set; }
}