using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "policy", "create")]
public record AzIotHubPolicyCreateOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--name")] string Name,
[property: CliOption("--permissions")] string Permissions
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}