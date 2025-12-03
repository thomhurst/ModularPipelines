using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "security-admin-config", "create")]
public record AzNetworkManagerSecurityAdminConfigCreateOptions(
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--apply-on")]
    public string? ApplyOn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}