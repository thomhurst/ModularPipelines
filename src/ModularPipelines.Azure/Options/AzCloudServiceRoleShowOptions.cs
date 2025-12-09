using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service", "role", "show")]
public record AzCloudServiceRoleShowOptions : AzOptions
{
    [CliOption("--cloud-service-name")]
    public string? CloudServiceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role-name")]
    public string? RoleName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}